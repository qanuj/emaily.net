using System;
using System.Linq;
using Emaily.Core.Abstraction.Persistence;
using Emaily.Core.Abstraction.Services;
using Emaily.Core.Data;
using Emaily.Core.DTO;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Amazon.SimpleEmail.Model;
using CsvHelper;
using CsvHelper.Configuration;
using Emaily.Core.Data.Complex;
using Emaily.Core.Enumerations;
using Newtonsoft.Json;

namespace Emaily.Services
{
    public class RegexUtilities
    {
        bool invalid = false;

        public bool IsValidEmail(string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }
    }

    public class EmailService : IEmailService
    {
        private readonly IRepository<App> _appRepository;
        private readonly IRepository<Plan> _planRepository;
        private readonly IRepository<AutoEmail> _autoEmailRepository;
        private readonly IRepository<AutoResponder> _autoResponderRepository;
        private readonly IRepository<Domain> _domainRepository; 
        private readonly IRepository<Template> _templateRepository;
        private readonly IRepository<Click> _clickRepository;
        private readonly IRepository<Client> _clientRepository;
        private readonly IRepository<NormalCampaign> _campaignRepository;
        private readonly IRepository<CampaignList> _campaignListRepository;
        private readonly IRepository<CampaignResult> _campaignResultRepository;
        private readonly IRepository<Link> _linkRepository;
        private readonly IRepository<Subscriber> _subscriberRepository;
        private readonly IRepository<List> _listRepository;
        private readonly IRepository<SubscriberReport> _subscriberReportRepository;
        private readonly IRepository<Promo> _promoRepository;
        private readonly IEmailProvider _emailProvider;
        private readonly ICloudProvider _cloudProvider;
        //private readonly IStorageProvider _storageProvider;
        private readonly IAppProvider _appProvider;
        private readonly INotificationHub _notificationHub; 


        public EmailService(
            IRepository<App> appRepository, 
            IRepository<Plan> planRepository, 
            IRepository<Client> clientRepository, 
            IRepository<NormalCampaign> campaignRepository, 
            IRepository<List> listRepository,
            IRepository<SubscriberReport> subscriberReportRepository,
            IRepository<AutoEmail> autoEmailRepository, 
            IRepository<AutoResponder> autoResponderRepository, 
            IRepository<Domain> domainRepository, 
            IRepository<Template> templateRepository, 
            IRepository<CampaignList> campaignListRepository, 
            IRepository<CampaignResult> campaignResultRepository, 
            IRepository<Link> linkRepository, 
            IRepository<Subscriber> subscriberRepository, 
            IRepository<Promo> promoRepository,
            IRepository<Click> clickRepository,
            IEmailProvider emailProvider, 
            ICloudProvider cloudProvider, 
            IAppProvider appProvider, 
            INotificationHub notificationHub)
        {
            _appRepository = appRepository;
            _planRepository = planRepository;
            _clientRepository = clientRepository;
            _campaignRepository = campaignRepository;
            _listRepository = listRepository;
            _autoEmailRepository = autoEmailRepository;
            _autoResponderRepository = autoResponderRepository;
            _domainRepository = domainRepository; 
            _templateRepository = templateRepository;
            _campaignListRepository = campaignListRepository;
            _campaignResultRepository = campaignResultRepository;
            _subscriberReportRepository = subscriberReportRepository;
            _linkRepository = linkRepository;
            _subscriberRepository = subscriberRepository;
            _promoRepository = promoRepository;
            _emailProvider = emailProvider;
            _cloudProvider = cloudProvider;
            _appProvider = appProvider;
            _notificationHub = notificationHub;
            _clickRepository = clickRepository;
        }
        public IQueryable<AppVM> Apps()
        {
            return _appRepository.All.Where(x => _appProvider.Apps.Contains(x.Id)).Select(x => new AppVM
            {
                FromEmail = x.Sender.Email,
                Id = x.Id,
                Name = x.Name,
                Plan = x.Plan.Name,
                PlanId = x.PlanId,
                Quota = x.Quota,
                Used = x.Used
            });
        }

        public TemplateInfoVM TemplateById(int id)
        {
            var template = _templateRepository.All.Where(x => x.Id == id).Select(x =>
            new TemplateInfoVM
            {
                AppId = x.AppId,
                Custom = x.Custom,
                Id = x.Id,
                Name = x.Name,
                HtmlText = x.HtmlText,
                PlainText = x.PlainText,
                QueryString = x.QueryString,
                Sender = new SenderViewModel
                {                     
                    ReplyTo = x.Sender.ReplyTo,
                    Email = x.Sender.Email,
                    Name = x.Sender.Name
                }
            }).FirstOrDefault();
            if (template == null) throw new Exception("Template not found");
            CheckIsMine(template.AppId);
            return template;
        }

        public bool DeleteTemplate(int id)
        {
            var template = _templateRepository.ById(id);
            if (template == null) throw new Exception("Template not found");
            CheckIsMine(template.AppId);

            _templateRepository.Delete(template);
            return _templateRepository.SaveChanges() > 0;
        }

        public ListInfoVM ListById(int id)
        {
            var list = _listRepository.All.Where(x => x.Id == id).Select(x =>
            new ListInfoVM
            {
                Id = x.Id,
                Name = x.Name,
                AppId = x.AppId,
                ConfirmUrl = x.ConfirmUrl,
                GoodBye = x.GoodBye,
                Confirmation = x.Confirmation,
                ThankYou = x.ThankYou,
                IsOptIn = x.IsOptIn,
                UnsubscribedUrl = x.UnsubscribedUrl,
                Custom = x.Custom
            }).FirstOrDefault();
            if (list == null) throw new Exception("List not found");
            CheckIsMine(list.AppId);
            return list;
        }

        public bool DeleteList(int id)
        {
            var list = _listRepository.ById(id);
            if (list == null) throw new Exception("List not found");
            CheckIsMine(list.AppId);

            _listRepository.Delete(list);
            return _listRepository.SaveChanges() > 0;
        }

        public IQueryable<ListVM> Lists()
        {
            return _listRepository.All.Where(x => _appProvider.Apps.Contains(x.AppId)).Select(x => new ListVM
            {
                Id = x.Id,
                Name = x.Name,
                AppId = x.AppId,
                Key = x.Key,
                Bounced = x.Subscribers.Count(y => y.IsBounced),
                Spams = x.Subscribers.Count(y => y.IsComplaint),
                Unsubscribed = x.Subscribers.Count(y => y.IsUnsubscribed),
                Actives = x.Subscribers.Count(y => !y.IsBounced && !y.IsComplaint && !y.IsUnsubscribed),
                Total = x.Subscribers.Count()
            });
        }
        public IQueryable<SubscriberVM> Subscribers(int listId)
        {
            return _subscriberRepository.All.Where(x => x.ListId== listId && _appProvider.Apps.Contains(x.AppId)).Select(x => new SubscriberVM
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Custom = x.Custom,
                Created = x.Created,
                IsBounced = x.IsBounced,
                IsComplaint = x.IsComplaint,
                IsConfirmed = x.IsConfirmed,
                IsSoftBounce = x.IsSoftBounce,
                IsUnsubscribed = x.IsUnsubscribed
            });
        }
        public IQueryable<CampaignVM> Campaigns()
        {
            return _campaignRepository.All.Where(x => _appProvider.Apps.Contains(x.Id)).Select(x => new CampaignVM
            {
                Errors = x.Errors,
                Id = x.Id,
                Title = x.Label=="" || x.Label==null ?  x.Name : x.Label,
                Status = x.Status,
                Recipients = x.Recipients,
                Started = x.Started,
                UniqueClicks = x.Links.Sum(z => z.Clicks.Select(y => y.SubscriberId).Distinct().Count()),
                UniqueOpens = x.Results.Select(y => y.SubscriberId).Distinct().Count()
            });
        }
        public IQueryable<CampaignReportVM> CampaignReports()
        {
            return _campaignRepository.All.Where(x => _appProvider.Apps.Contains(x.Id) &&  x.Started.HasValue).Select(x => new CampaignReportVM
            {
                Id = x.Id,
                Title = x.Label == "" || x.Label == null ? x.Name : x.Label,
                Status = x.Status,
                Recipients = x.Recipients,
                Started = x.Started.Value,
                UniqueClicks = x.Links.Sum(z => z.Clicks.Select(y => y.SubscriberId).Distinct().Count()),
                UniqueOpens = x.Results.Select(y => y.SubscriberId).Distinct().Count()
            });
        }

        private void UpdateSubcriberReport(List list, int addedOrRemoved)
        {
            if (addedOrRemoved == 0) return;
            var lastOne=_subscriberReportRepository.All.OrderByDescending(x => x.Id).FirstOrDefault(x => x.ListId == list.Id);
            if (lastOne != null)
            {
                addedOrRemoved += lastOne.Total;
            }
            list.TotalRecord = addedOrRemoved;
            _listRepository.Update(list);
            _subscriberReportRepository.Create(new SubscriberReport(addedOrRemoved,list.Id));
            _subscriberReportRepository.SaveChanges();
            _notificationHub.Notify(list.OwnerId,NotificationTypeEnum.Import, new {list=list.Id, total=list.TotalRecord });
        }

        public SubscriberVM UpdateSubscriber(UpdateSubscriberVM model)
        {
            throw new NotImplementedException();
        }

        public SubscriberVM SubscriberById(int list, int id)
        {
            return Subscribers(list).FirstOrDefault(x => x.Id == id);
        }

        public bool DeleteSubscriber(int list, int id)
        {
            var subscriber = _subscriberRepository.ById(id);
            var listOf = _listRepository.ById(list);
            if (subscriber == null || subscriber.ListId!=list) throw new Exception("List not found");
            CheckIsMine(listOf.AppId);

            _subscriberRepository.Delete(subscriber);
            return _subscriberRepository.SaveChanges() > 0;
        }

        public IQueryable<TemplateVM> Templates()
        {
            return _templateRepository.All.Where(x => _appProvider.Apps.Contains(x.Id)).Select(x => new TemplateVM
            {
                Id = x.Id,
                Title = x.Name,
                Created = x.Created
            });
        }

        private Plan FindPlanByName(string name)
        {
            return _planRepository.All.FirstOrDefault(x => x.Name == name);
        }
        private Client FindClientByOwnerId(string ownerId)
        {
            return _clientRepository.All.FirstOrDefault(x => x.OwnerId == ownerId);
        }
        private string GenerateRandomString(int length)
        {
            return "hello";
        }
        private string GenerateRandomString(string name)
        {
            return "hello";
        }
        private void SendNote(MailNote note, EmailAddress sender, EmailAddress receiver)
        {
            _emailProvider.SendEmail(sender, receiver, note);
        }
        private void CheckIsMine(int appId)
        {
            if (appId <= 0) throw new ArgumentOutOfRangeException(nameof(appId));
            if (!_appProvider.Apps.Any(x => appId == x)) throw new Exception("Access Denied");
        }

        private CampaignResult CreateResult(CampaignResultVM model, string userAgent, string country)
        {
            return _campaignResultRepository.Create(new CampaignResult
            {
                Opened = DateTime.UtcNow,
                SubscriberId = model.SubscriberId,
                CampaignId = model.CampaignId,
                Country = country,
                UserAgent = userAgent,
            });
        }

        public ListVM CreateList(CreateListVM model)
        {
            var app = _appRepository.ById(model.AppId);
            if (app == null) throw new Exception("App not found");
            CheckIsMine(model.AppId);

            if (_listRepository.All.Any(x => x.AppId == model.AppId && x.Name == model.Name)) throw new Exception("List already exists");

            var list = _listRepository.Create(new List { AppId = model.AppId, Name = model.Name, Key=GenerateRandomString(model.Name) });
            _listRepository.SaveChanges();

            return Lists().FirstOrDefault(x => x.Id == list.Id);
        }

        public void Subscribe(CreateSubscriber model)
        {
            SubscribeInternal(model, _listRepository.ById(model.ListId));
        }

        private bool IsValidEmail(string email)
        {
            return !(string.IsNullOrWhiteSpace(email) || !new RegexUtilities().IsValidEmail(email));
        }
       
        private void SubscribeInternal(CreateSubscriber model,List list, bool updateCounts=true, bool sendEmail=true)
        {
            if (list == null) throw new Exception("List not found");

            model.Email = model.Email.ToLower().Trim();
            if (!IsValidEmail(model.Email)) throw new Exception("Invalid Email address");

            var app = _appRepository.ById(list.AppId);
            if (_subscriberRepository.All.Any(x => x.Email == model.Email && x.ListId==list.Id)) throw new Exception("Already subscribed");
            if (_subscriberRepository.All.Any(x => x.Email == model.Email && x.IsBounced)) throw new Exception("Already bounced");

            var subscriber = _subscriberRepository.Create(new Subscriber
            {
                AppId = list.AppId,
                Custom = JsonConvert.SerializeObject(model.Custom),
                Email = model.Email,
                Name = model.Name,
                ListId = model.ListId,
                OwnerId = _appProvider.OwnerId,
                Token=GenerateRandomString(model.Email)
            });

            if (updateCounts)
            {
                UpdateSubcriberReport(list, 1);
                _subscriberRepository.SaveChanges();
            }

            if (sendEmail)
            {
                if (list.IsOptIn && list.Confirmation.IsActive)
                {
                    SendNote(list.Confirmation, app.Sender, new EmailAddress(subscriber.Email, subscriber.Name));
                }
                else if (list.ThankYou.IsActive)
                {
                    SendNote(list.ThankYou, app.Sender, new EmailAddress(subscriber.Email, subscriber.Name));
                }
            }
        }

        private const int ImportEvery=125;  

        private Stream GenerateStreamFromString(string text)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(text);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public ImportResult ImportSubscribers(TextReader reader, int listId)
        {
            var result = new ImportResult { };
            var list = _listRepository.ById(listId);
            if (list == null) throw new Exception("List not found");
            Task.Run(()=>
            {
                using (var csv = new CsvReader(reader,
                    new CsvConfiguration()
                    {
                        IsHeaderCaseSensitive = true,
                        Encoding = Encoding.UTF8,
                        IgnoreBlankLines = true,

                    }))
                {
                    while (csv.Read())
                    {
                        try
                        {
                            var item = csv.GetRecord<dynamic>();
                            var record = new ListEmail
                            {
                                Email = item.Email,
                                Name = item.Name
                            };

                            if (!IsValidEmail(record.Email)) continue;

                            var dict = (IDictionary<string, object>)item;
                            if (dict.ContainsKey("Email")) dict.Remove("Email");
                            if (dict.ContainsKey("Name")) dict.Remove("Name");

                            SubscribeInternal(new CreateSubscriber
                            {
                                Name = record.Name,
                                Email = record.Email,
                                ListId = listId,
                                Custom = item
                            }, list, false, false);
                            result.Added++;

                            if (result.Added % ImportEvery == 0)
                            {
                                UpdateSubcriberReport(list, result.Added);
                                _subscriberRepository.SaveChanges();
                                result.Added = 0;
                            }
                        }
                        catch (Exception ex)
                        {
                            result.Failed++;
                        }
                    }
                }
                UpdateSubcriberReport(list, result.Added);
                _subscriberRepository.SaveChanges();
            });
           
            return result;
        }

        public ImportResult ImportSubscribers(string importData, int listId)
        {
            return ImportSubscribers(new StreamReader(GenerateStreamFromString(importData)), listId);
        }

        public void ConfirmSubscription(UpdateSubscriptionVM model)
        {
            model.Email = model.Email.ToLower().Trim();
            if (string.IsNullOrWhiteSpace(model.Email)) throw new Exception("Invalid Email address");

            var subscriber = _subscriberRepository.ById(model.Id);
            if (subscriber == null) throw new Exception("Subscriber not found");
            if (subscriber.Token != model.Token) throw  new Exception("Invalid token received");

            var app = _appRepository.ById(subscriber.AppId);
            var list = _listRepository.ById(subscriber.ListId);

            subscriber.IsConfirmed = true;
            _subscriberRepository.Update(subscriber);
            _subscriberRepository.SaveChanges();

            if (list.ThankYou.IsActive)
            {
                SendNote(list.ThankYou, app.Sender, new EmailAddress(subscriber.Email, subscriber.Name));
            }
        }
        public void Unsubscribe(ListEmail model)
        {
            model.Email = model.Email.ToLower().Trim();
            if(string.IsNullOrWhiteSpace(model.Email)) throw new Exception("Invalid Email address");

            var subscriber = _subscriberRepository.All.FirstOrDefault(x => x.Email == model.Email && x.ListId == model.ListId && !x.IsBounced && !x.IsComplaint && !x.IsUnsubscribed);
            if (subscriber==null) throw new Exception("Already unsubscribed");

            var list = _listRepository.ById(model.ListId);

            subscriber.IsUnsubscribed = true;
            _subscriberRepository.Update(subscriber);
            _subscriberRepository.SaveChanges();

            if (list == null || !list.GoodBye.IsActive) return;

            var app = _appRepository.ById(list.AppId);
            SendNote(list.GoodBye, app.Sender,new EmailAddress(subscriber.Email, subscriber.Name));
        }
        public ListVM RenameList(RenameListVM model)
        {
            var list = _listRepository.ById(model.Id);
            if (list == null) throw new Exception("List not found");
            CheckIsMine(list.AppId);

            list.Name = model.Name;

            _listRepository.Update(list);
            _listRepository.SaveChanges();

            return Lists().FirstOrDefault(x => x.Id == list.Id);
        }
        public ListVM UpdateList(UpdateListVM model)
        {
            var list = _listRepository.ById(model.Id);
            if (list == null) throw new Exception("List not found");
            CheckIsMine(list.AppId);

            list.Confirmation = model.Confirmation;
            list.GoodBye = model.GoodBye;
            list.ThankYou = model.ThankYou;
            list.SubscribedUrl = model.SubscribedUrl;
            list.ConfirmUrl = model.ConfirmUrl;
            list.IsOptIn = model.IsOptIn;
            list.IsUnsubcribeAllList = model.IsUnsubcribeAllList;
            list.UnsubscribedUrl = model.UnsubscribedUrl;

            _listRepository.Update(list);
            _listRepository.SaveChanges();

            return Lists().FirstOrDefault(x => x.Id == list.Id);
        }
        public AppVM UpdateApp(UpdateAppVM model)
        {
            var app = _appRepository.ById(model.Id);
            if (app == null) throw new Exception("App not found");
            CheckIsMine(model.Id);

            app.Name = model.Company;
            app.Sender.Name = model.FromName;
            app.Sender.ReplyTo = model.ReplyTo;
            app.Logo = model.Logo;
            app.Smtp = model.Smtp;

            _appRepository.SaveChanges();

            return Apps().FirstOrDefault(x => x.Id == app.Id);
        }
        public void SendCampaign(SendCampaignVM model)
        {
            var campaign = _campaignRepository.ById(model.CampaignId);
            if (campaign == null) throw new Exception("Campaign not found");
            CheckIsMine(campaign.AppId);

            var domain = campaign.Sender.Email.Split('@')[1];
            var verification = _domainRepository.All.FirstOrDefault(x=>x.Verified.HasValue && (x.Name == campaign.Sender.Email || x.Name== domain));
            if (verification == null) throw new Exception("Domain / Email not verified");

            campaign.Started = DateTime.UtcNow;
            campaign.Recipients = 0;
            campaign.Status = CampaignStatusEnum.Active;
            campaign.Future = model.Future;
            campaign.TimeZone = model.TimeZone;

            var lists = _listRepository.ById(model.Lists);
            foreach (var list in lists)
            {
                campaign.Recipients += list.TotalRecord;
                _campaignListRepository.Create(new CampaignList
                {
                   CampaignId = campaign.Id,
                   ListId = list.Id
                });
            }

            _campaignRepository.SaveChanges();
        }
        public CampaignVM CreateCampaign(CreateCampaignVM model)
        {
            var app = _appRepository.ById(model.AppId);
            if (app == null) throw new Exception("App not found");
            CheckIsMine(app.Id);

            var campaign = _campaignRepository.Create(new NormalCampaign
            {
                Name = model.Name,
                Label = model.Label,
                Status = CampaignStatusEnum.Draft,
                Sender = new EmailAddress
                {
                    Name = model.FromName,
                    Email = app.Sender.Email,
                    ReplyTo = model.ReplyTo
                },
                AppId = app.Id,
                HtmlText = model.HtmlText,
                PlainText = model.PlainText,
                IsHtml = model.IsHtml,
                OwnerId = _appProvider.OwnerId,
                QueryString = model.QueryString
            });

            _campaignRepository.SaveChanges();

            return Campaigns().FirstOrDefault(x => x.Id == campaign.Id);
        }
        public CampaignVM UpdateCampaign(EditCampaignVM model)
        {
            var campaign = _campaignRepository.ById(model.CampaignId);
            if (campaign == null) throw new Exception("Campaign not found");
            CheckIsMine(campaign.AppId);

            campaign.Name = model.Name;
            campaign.Label = model.Label;
            campaign.Status = CampaignStatusEnum.Draft;
            campaign.Sender.Name = model.FromName;
            campaign.Sender.ReplyTo = model.ReplyTo;
            campaign.HtmlText = model.HtmlText;
            campaign.PlainText = model.PlainText;
            campaign.IsHtml = model.IsHtml;
            campaign.QueryString = model.QueryString;

            _campaignRepository.Update(campaign);
            _campaignRepository.SaveChanges();

            return Campaigns().FirstOrDefault(x => x.Id == campaign.Id);
        }

        public bool DeleteCampaign(int id)
        {
            var campaign = _campaignRepository.ById(id);
            if (campaign == null) throw new Exception("Campaign not found");
            CheckIsMine(campaign.AppId);

            _campaignRepository.Delete(campaign);
            return _campaignRepository.SaveChanges()>0;
        }

        public CampaignInfoVM CampaignById(int id)
        {
            var campaign = _campaignRepository.All.Where(x => x.Id == id).Select(x =>
            new CampaignInfoVM
            {
                AppId=x.AppId,
                Custom = x.Custom,
                Id = x.Id,
                Title = x.Name,
                Label = x.Label,
                Future = x.Future,
                HtmlText = x.HtmlText,
                IsHtml = x.IsHtml,
                Lists = x.Lists.Select(y=>y.ListId),
                PlainText = x.PlainText,
                Sender = new SenderViewModel
                {
                     Email = x.Sender.Email,
                     Name = x.Sender.Name,
                     ReplyTo = x.Sender.ReplyTo
                },
                QueryString = x.QueryString
            }).FirstOrDefault();
            if (campaign == null) throw new Exception("Campaign not found");
            CheckIsMine(campaign.AppId);
            return campaign;
        }

        public AppVM CreateApp(CreateAppVM model)
        {
            var plan = FindPlanByName(model.Plan);
            if (plan == null) throw new Exception("Plan not found");

            int? promoId = null;
            if (!string.IsNullOrWhiteSpace(model.PromoCode))
            {
                var promo =
                    _promoRepository.All.FirstOrDefault(
                        x =>
                            x.Code == model.PromoCode && (!x.Start.HasValue || x.Start < DateTime.UtcNow) &&
                            (!x.End.HasValue || x.End > DateTime.UtcNow));

                if(promo==null) throw new Exception("Invalid Promo Code");

                promoId = promo.Id;
            }

            var app = _appRepository.Create(new App
            {
                PlanId = plan.Id,
                PromoId = promoId,
                AppKey = GenerateRandomString(16),
                Clients=new List<Client>
                {
                    new Client
                    {
                        ApiKey = GenerateRandomString(16),
                        IsOwner = true,
                        OwnerId = _appProvider.OwnerId,
                        Name = model.Company
                    }
                },
                Name = model.Company,
                Currency = model.Currency,
                Smtp = model.Smtp,
                Sender= model.Sender,
                Logo = model.Logo,
                Quota = plan.Quota
            });

            _cloudProvider.VerifyEmail(model.Sender.Email);

            _appRepository.SaveChanges();

            return Apps().FirstOrDefault(x => x.Id == app.Id);
        }
        public void MarkRead(CampaignResultVM model, string country, string userAgent)
        {
            var subscriber = _subscriberRepository.ById(model.SubscriberId);
            if (subscriber == null) throw new Exception("Subscriber not found");

            var campaignResult = _campaignResultRepository.All.FirstOrDefault(x => x.CampaignId == model.CampaignId && x.SubscriberId == model.SubscriberId) ??
                            CreateResult(model, userAgent, country);

            campaignResult.Opened = DateTime.UtcNow;

            _campaignResultRepository.Update(campaignResult);
            _campaignResultRepository.SaveChanges();
        }
        public void MarkSpam(CampaignResultVM model, string country, string userAgent)
        {
            var subscriber = _subscriberRepository.ById(model.SubscriberId);
            if (subscriber == null) throw new Exception("Subscriber not found");

            subscriber.IsComplaint = true;

            var campaignResult = _campaignResultRepository.All.FirstOrDefault(x => x.CampaignId == model.CampaignId && x.SubscriberId == model.SubscriberId) ??
                            CreateResult(model, userAgent, country);

            campaignResult.MarkedSpam = DateTime.UtcNow;

            _campaignResultRepository.Update(campaignResult);
            _campaignResultRepository.SaveChanges();
        }
        public void MarkBounced(CampaignResultVM model, string country, string userAgent, bool IsSoftBounce)
        {
            var subscriber = _subscriberRepository.ById(model.SubscriberId);
            if (subscriber == null) throw new Exception("Subscriber not found");

            subscriber.IsBounced = true;
            subscriber.IsSoftBounce = IsSoftBounce;

            var campaignResult = _campaignResultRepository.All.FirstOrDefault(x => x.CampaignId == model.CampaignId && x.SubscriberId == model.SubscriberId) ??
                            CreateResult(model, userAgent, country);

            campaignResult.Bounced = DateTime.UtcNow;
            campaignResult.Opened = null; //since it bounced;
            campaignResult.IsSoftBounce = IsSoftBounce;


            _campaignResultRepository.Update(campaignResult);
            _campaignResultRepository.SaveChanges();
        }
        public void CreateOrUpdateClick(CreateClickVM model,string country,string userAgent)
        {
            var campaign = _campaignRepository.ById(model.CampaignId);
            if (campaign == null) throw new Exception("Campaign not found");

            MarkRead(model, country, userAgent);

            var link = _linkRepository.ById(model.LinkId);
            if (link == null) throw new Exception("Link not found");

            _clickRepository.Create(new Click
            {
                SubscriberId = model.SubscriberId,
                LinkId = model.LinkId
            });

            _clickRepository.SaveChanges(); //incase the open has not fired.

        }
        public TemplateVM CreateTemplate(CreateTemplateVM model)
        {
            var app = _appRepository.ById(model.AppId);
            if (app == null) throw new Exception("App not found");
            CheckIsMine(app.Id);

            var template=_templateRepository.Create(new Template
            {
               AppId = model.AppId,
               Name = model.Name,
               Custom = JsonConvert.SerializeObject(model.Custom),
                HtmlText = model.HtmlText,
                PlainText = model.PlainText,
                QueryString = model.QueryString,
                OwnerId = _appProvider.OwnerId
            });
            _templateRepository.SaveChanges();

            return Templates().FirstOrDefault(x => x.Id == template.Id);

        }
        public TemplateVM UpdateTemplate(UpdateTemplateVM model)
        {
            var template = _templateRepository.ById(model.Id);
            if (template == null) throw new Exception("Template not found");
            CheckIsMine(template.AppId);

            template.Name = model.Name;
            template.HtmlText = model.HtmlText;
            template.PlainText = model.PlainText;
            template.QueryString = model.QueryString;
            template.Sender= new EmailAddress
            {
                Email = model.Sender.Email,
                Name = model.Sender.Name,
                ReplyTo = model.Sender.ReplyTo
            };
            template.Custom = JsonConvert.SerializeObject(model.Custom);

            _templateRepository.Update(template);

            _templateRepository.SaveChanges();

            return Templates().FirstOrDefault(x => x.Id == template.Id);
        }
        public void CreateAutoEmail(CreateAutoEmailVM model)
        {
            var autoResponder = _autoResponderRepository.ById(model.AutoResponderId);
            if (autoResponder == null) throw new Exception("Auto Responder not found");

            var list = _listRepository.ById(autoResponder.ListId);
            if (list == null) throw new Exception("List not found");

            var app = _appRepository.ById(list.AppId);
            if (app == null) throw new Exception("App not found");

            _autoEmailRepository.Create(new AutoEmail
            {
               AutoResponderId = model.AutoResponderId,
               AppId = list.AppId,
               Name = model.Name,
               Custom = JsonConvert.SerializeObject(model.Custom),
               TimeCondition=model.TimeCondition,
               TimeZone = model.TimeZone,
               HtmlText = model.HtmlText,
               IsHtml = model.IsHtml,
               Label = model.Label,
               PlainText = model.PlainText,
               OwnerId = _appProvider.OwnerId,
               QueryString = model.QueryString,
               Sender = new EmailAddress
               {
                    Name = model.FromName,
                    Email = app.Sender.Email,
                    ReplyTo = model.ReplyTo
               },
               Status = CampaignStatusEnum.Active
            });

            _autoResponderRepository.SaveChanges();

        }
        public void UpdateAutoEmail(UpdateAutoEmailVM model)
        {
            var autoEmail = _autoEmailRepository.ById(model.Id);
            if (autoEmail == null) throw new Exception("Auto Responder Email not found");

            var autoResponder = _autoResponderRepository.ById(autoEmail.AutoResponderId);
            if (autoResponder == null) throw new Exception("Auto Responder not found");

            autoEmail.Name = model.Name;
            autoEmail.Label = model.Label;
            autoEmail.Sender.Name = model.FromName;
            autoEmail.Sender.ReplyTo = model.ReplyTo;
            autoEmail.HtmlText = model.HtmlText;
            autoEmail.PlainText = model.PlainText;
            autoEmail.IsHtml = model.IsHtml;
            autoEmail.QueryString = model.QueryString;
            autoEmail.TimeCondition = model.TimeCondition;
            autoEmail.TimeZone = model.TimeZone;
            autoEmail.Custom = JsonConvert.SerializeObject(model.Custom);

            _autoEmailRepository.Update(autoEmail);
            _autoResponderRepository.SaveChanges();
        }
        public void CreateAutoResponder(CreateAutoResponderVM model)
        {
            var list = _listRepository.ById(model.ListId);
            if (list == null) throw new Exception("List not found");

            CheckIsMine(list.AppId);

            _autoResponderRepository.Create(new AutoResponder
            {
               Name = model.Name,
               Custom = JsonConvert.SerializeObject(model.Custom),
               ListId = model.ListId,
               Mode = model.Mode
            });

            _autoResponderRepository.SaveChanges();
        }
        public void UpdateAutoResponder(UpdateAutoResponderVM model)
        {
            var entity = _autoResponderRepository.ById(model.Id);
            if (entity == null) throw new Exception("Auto Responder not found");

            var list = _listRepository.ById(entity.ListId);
            if (list == null) throw new Exception("List not found");

            CheckIsMine(list.AppId);

            entity.Mode = model.Mode;
            entity.Name = model.Name;
            entity.Custom = JsonConvert.SerializeObject(model.Custom);

            _autoResponderRepository.Update(entity);

            _autoResponderRepository.SaveChanges();
        }

        public void AddCustomField(CustomFieldVM model)
        {
            var list = _listRepository.ById(model.ListId);
            if (list == null) throw new Exception("List not found");
            CheckIsMine(list.AppId);

            var fields = string.IsNullOrWhiteSpace(list.Custom) ? new List<CustomField>() : JsonConvert.DeserializeObject<List<CustomField>>(list.Custom);

            if (fields.All(x => x.Name != model.Name))
            {
                fields.Add(new CustomField { Name = model.Name, Mode = model.Mode });
            }
            list.Custom = JsonConvert.SerializeObject(fields);

            _listRepository.Update(list);
            _listRepository.SaveChanges();
        }

        public void RenameCustomField(RenameCustomFieldVM model)
        {
            var list = _listRepository.ById(model.ListId);
            if (list == null) throw new Exception("List not found");
            CheckIsMine(list.AppId);

            var fields = string.IsNullOrWhiteSpace(list.Custom) ? new List<CustomField>() : JsonConvert.DeserializeObject<List<CustomField>>(list.Custom);
            var field = fields.FirstOrDefault(x => x.Name == model.OldName);
            if (field != null && fields.All(x => x.Name != model.NewName))
            {
                field.Name = model.NewName;
            }
            list.Custom = JsonConvert.SerializeObject(fields);
            _listRepository.Update(list);
            _listRepository.SaveChanges();
        }

        public void DeleteCustomField(CustomFieldVM model)
        {
            var list = _listRepository.ById(model.ListId);
            if (list == null) throw new Exception("List not found");
            CheckIsMine(list.AppId);

            var fields = string.IsNullOrWhiteSpace(list.Custom) ? new List<CustomField>() : JsonConvert.DeserializeObject<List<CustomField>>(list.Custom);
            var field = fields.FirstOrDefault(x => x.Name == model.Name);
            if (field != null){
                fields.Remove(field);
            }
            list.Custom = JsonConvert.SerializeObject(fields);
            _listRepository.Update(list);
            _listRepository.SaveChanges();
        }
    }
}
