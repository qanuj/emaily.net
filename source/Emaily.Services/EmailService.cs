using System;
using System.Linq;
using Emaily.Core.Abstraction.Persistence;
using Emaily.Core.Abstraction.Services;
using Emaily.Core.Data;
using Emaily.Core.DTO;
using System.Collections.Generic;
using System.Net.Configuration;
using System.Net.Mail;
using System.Reflection.Emit;
using System.Text;
using Amazon.SimpleEmail.Model.Internal.MarshallTransformations;
using Emaily.Core.Data.Complex;
using Emaily.Core.Enumerations;
using Newtonsoft.Json;

namespace Emaily.Services
{
    public class CreateAppVM
    {
        public string Company { get; set; }
        public string Email { get; set; }
        public string Plan { get; set; }
        public string OwnerId { get; set; }
        public CurrencyEnum Currency { get; set; }
        public SmtpInfo Smtp { get; set; }
        public EmailAddress Sender { get; set; }
        public string Logo { get; set; }
    }

    public class UpdateAppVM
    {
        public string Company { get; set; }
        public SmtpInfo Smtp { get; set; }
        public string FromName { get; set; }
        public string ReplyTo { get; set; }
        public string Logo { get; set; }
        public int Id { get; set; }
    }

    public class CampaignVM
    {
        public int Id { get; set; }     
        public string Errors { get; set; }
        public string Title { get; set; }
        public int Recipients { get; set; }
        public DateTime? Started { get; set; }
        public int UniqueOpens { get; set; }
        public int UniqueClicks { get; set; }
        public CampaignStatusEnum Status { get; set; }
    }

   public class CreateCampaignVM
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string HtmlText { get; set; }
        public string PlainText { get; set; }
        public bool IsHtml { get; set; }
        public string FromName { get; set; }
        public string ReplyTo { get; set; }
        public string QueryString { get; set; }
        public string Label { get; set; }
        public int AppId { get; set; }
        public string OwnerId { get; set; }
    }

    public class EditCampaignVM 
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string HtmlText { get; set; }
        public string PlainText { get; set; }
        public bool IsHtml { get; set; }
        public string FromName { get; set; }
        public string ReplyTo { get; set; }
        public string QueryString { get; set; }
        public string Label { get; set; }
        public string OwnerId { get; set; }
        public int CampaignId { get; set; }
    }

    public class SendCampaignVM
    {
        public int CampaignId { get; set; }
        public int[] Lists { get; set; }
        public DateTime? Future { get; set; }
        public string Timezone { get; set; }
    }

    public class ListVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AppId { get; set; }
        public int Actives { get; set; }
        public int Unsubscribed { get; set; }
        public int Bounced { get; set; }
        public int Total { get; set; }
        public string Key { get; set; }
        public int Spams { get; set; }
    }

    public class CreateListVM
    {
        public int AppId { get; set; }
        public string Name { get; set; }
    }

    public class RenameListVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateListVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MailNote Confirmation { get; set; }
        public MailNote GoodBye { get; set; }
        public MailNote ThankYou { get; set; }
        public string SubscribedUrl { get; set; }
        public string ConfirmUrl { get; set; }
        public bool IsOptIn { get; set; }
        public bool IsUnsubcribeAllList { get; set; }
        public string UnsubscribedUrl { get; set; }
    }

    public class CreateSubscriber
    {
        public int ListId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public dynamic Custom { get; set; }
        public string OwnerId { get; set; }
    }

    public class ListEmail
    {
        public int ListId { get; set; }
        public string Email { get; set; }
    }

    public class UpdateSubscriptionVM
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class EmailService : IEmailService
    {
        private readonly IRepository<App> _appRepository;
        private readonly IRepository<Plan> _planRepository;
        private readonly IRepository<AutoEmail> _autoEmailRepository;
        private readonly IRepository<AutoResponder> _autoResponderRepository;
        private readonly IRepository<Domain> _domainRepository;
        private readonly IRepository<Queue> _queueRepository;
        private readonly IRepository<Template> _templateRepository;
        private readonly IRepository<Click> _clickRepository;
        private readonly IRepository<Client> _clientRepository;
        private readonly IRepository<Campaign> _campaignRepository;
        private readonly IRepository<CampaignList> _campaignListRepository;
        private readonly IRepository<CampaignResult> _campaignResultRepository;
        private readonly IRepository<Link> _linkRepository;
        private readonly IRepository<Subscriber> _subscriberRepository;
        private readonly IRepository<List> _listRepository;
        private readonly IRepository<Promo> _promoRepository;
        private readonly IEmailProvider _emailProvider;
        private readonly ICloudProvider _cloudProvider;
        private readonly IStorageProvider _storageProvider;
        private readonly IAppProvider _appProvider;


        public EmailService(IRepository<App> appRepository, IRepository<Plan> planRepository, IRepository<Client> clientRepository, IRepository<Campaign> campaignRepository, IRepository<List> listRepository, IRepository<AutoEmail> autoEmailRepository, IRepository<AutoResponder> autoResponderRepository, IRepository<Domain> domainRepository, IRepository<Queue> queueRepository, IRepository<Template> templateRepository, IRepository<CampaignList> campaignListRepository, IRepository<CampaignResult> campaignResultRepository, IRepository<Link> linkRepository, IRepository<Subscriber> subscriberRepository, IRepository<Promo> promoRepository, IEmailProvider emailProvider, IStorageProvider storageProvider, ICloudProvider cloudProvider, IAppProvider appProvider, IRepository<Click> clickRepository)
        {
            _appRepository = appRepository;
            _planRepository = planRepository;
            _clientRepository = clientRepository;
            _campaignRepository = campaignRepository;
            _listRepository = listRepository;
            _autoEmailRepository = autoEmailRepository;
            _autoResponderRepository = autoResponderRepository;
            _domainRepository = domainRepository;
            _queueRepository = queueRepository;
            _templateRepository = templateRepository;
            _campaignListRepository = campaignListRepository;
            _campaignResultRepository = campaignResultRepository;
            _linkRepository = linkRepository;
            _subscriberRepository = subscriberRepository;
            _promoRepository = promoRepository;
            _emailProvider = emailProvider;
            _storageProvider = storageProvider;
            _cloudProvider = cloudProvider;
            _appProvider = appProvider;
            _clickRepository = clickRepository;
        }

        private IQueryable<AppVM> Apps()
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

        private IQueryable<ListVM> Lists()
        {
            return _listRepository.All.Where(x=> _appProvider.Apps.Contains(x.AppId)).Select(x => new ListVM
            {
                Id = x.Id,
                Name = x.Name,
                AppId=x.AppId,
                Key = x.Key,
                Bounced = x.Subscribers.Count(y => y.IsBounced),
                Spams = x.Subscribers.Count(y => y.IsComplaint),
                Unsubscribed = x.Subscribers.Count(y => y.IsUnsubscribed),
                Actives = x.Subscribers.Count(y => !y.IsBounced && !y.IsComplaint && !y.IsUnsubscribed),
                Total = x.Subscribers.Count()
            });
        }

        private IQueryable<CampaignVM> Campaigns()
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

        private Plan FindPlanByName(string name)
        {
            return _planRepository.All.FirstOrDefault(x => x.Name == name);
        }

        private Client FindClientByOwnerId(string ownerId)
        {
            return _clientRepository.All.FirstOrDefault(x => x.OwnerId == ownerId);
        }

        public string GenerateRandomString(int length)
        {
            return "hello";
        }

        public string GenerateRandomString(string name)
        {
            return "hello";
        }

        private void CheckIsMine(int appId)
        {
            if (_appProvider.Apps.Any(x => appId == x)) throw new Exception("Access Denied");
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
            model.Email = model.Email.ToLower().Trim();
            if (string.IsNullOrWhiteSpace(model.Email)) throw new Exception("Invalid Email address");

            var list = _listRepository.ById(model.ListId);
            if (list == null) throw new Exception("List not found");

            var app = _appRepository.ById(list.AppId);
            if (list.Subscribers.Any(x => x.Email == model.Email)) throw new Exception("Already subscribed");
            if (_subscriberRepository.All.Any(x => x.Email == model.Email && x.IsBounced)) throw new Exception("Already bounced");

            var subscriber = _subscriberRepository.Create(new Subscriber
            {
                AppId = list.AppId,
                Custom = JsonConvert.SerializeObject(model.Custom),
                Email = model.Email,
                Name = model.Name,
                ListId = model.ListId,
                OwnerId = model.OwnerId,
                Token=GenerateRandomString(model.Email)
            });

            _subscriberRepository.SaveChanges();

            if(list.IsOptIn && list.Confirmation.IsActive)
            {
                SendNote(list.Confirmation, app.Sender, new EmailAddress(subscriber.Email, subscriber.Name));
            }else if (list.ThankYou.IsActive)
            {
                SendNote(list.ThankYou, app.Sender, new EmailAddress(subscriber.Email, subscriber.Name));
            }

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

        private void SendNote(MailNote note, EmailAddress sender, EmailAddress receiver)
        {
            _emailProvider.SendEmail(sender, receiver,note);
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
            campaign.Status = CampaignStatusEnum.Start;
            campaign.Future = model.Future;
            campaign.Timezone = model.Timezone;

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

            var campaign = _campaignRepository.Create(new Campaign
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
                OwnerId = model.OwnerId,
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

        public AppVM CreateApp(CreateAppVM model)
        {
            var plan = FindPlanByName(model.Plan);
            if (plan == null) throw new Exception("Plan not found");

            var app = _appRepository.Create(new App
            {
                PlanId = plan.Id,
                OwnerId = model.OwnerId,
                AppKey = GenerateRandomString(16),
                Clients=new List<Client>
                {
                    new Client
                    {
                        ApiKey = GenerateRandomString(16),
                        IsOwner = true,
                        OwnerId = model.OwnerId,
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

        private CampaignResult CreateResult(CampaignResultVM model,string userAgent,string country)
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

        public void CreateTemplate(CreateTemplateVM model)
        {
            var template = _templateRepository.Create(new Template
            {
               AppId = model.AppId,
               Name = model.Name,
               Custom = JsonConvert.SerializeObject(model.Custom),
               Html = model.Html,
               OwnerId = model.OwnerId
            });
            _templateRepository.SaveChanges();

        }

        public void UpdateTemplate(UpdateTemplateVM model)
        {
            var template = _templateRepository.ById(model.Id);
            if (template == null) throw new Exception("Template not found");
            CheckIsMine(template.AppId);

            template.Name = model.Name;
            template.Html = model.Html;
            template.Custom = JsonConvert.SerializeObject(model.Custom);

            _templateRepository.Update(template);

            _templateRepository.SaveChanges();
        }
    }

    public class UpdateTemplateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public dynamic Custom { get; set; }
        public string Html { get; set; }
    }

    public class CreateTemplateVM
    {
        public int AppId { get; set; }
        public string Name { get; set; }
        public dynamic Custom { get; set; }
        public string Html { get; set; }
        public string OwnerId { get; set; }
    }

    public class CampaignResultVM
    {
        public int CampaignId { get; set; }
        public int SubscriberId { get; set; }
    }

    public class CreateClickVM  : CampaignResultVM
    {
        public int LinkId { get; set; }
    }
}
