using System;
using System.Linq;
using Emaily.Core.Abstraction.Persistence;
using Emaily.Core.Abstraction.Services;
using Emaily.Core.Data;
using Emaily.Core.DTO;
using System.Collections.Generic;
using Emaily.Core.Data.Complex;
using Emaily.Core.Enumerations;
using Newtonsoft.Json;

namespace Emaily.Services
{
    
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


        public EmailService(IRepository<App> appRepository, IRepository<Plan> planRepository, IRepository<Client> clientRepository, IRepository<Campaign> campaignRepository, IRepository<List> listRepository, IRepository<AutoEmail> autoEmailRepository, IRepository<AutoResponder> autoResponderRepository, IRepository<Domain> domainRepository,
            IRepository<Template> templateRepository, IRepository<CampaignList> campaignListRepository, IRepository<CampaignResult> campaignResultRepository, IRepository<Link> linkRepository, IRepository<Subscriber> subscriberRepository, IRepository<Promo> promoRepository, IEmailProvider emailProvider, IStorageProvider storageProvider, ICloudProvider cloudProvider, IAppProvider appProvider, IRepository<Click> clickRepository)
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
                OwnerId = _appProvider.OwnerId,
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
            campaign.Status = CampaignStatusEnum.Active;
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
                OwnerId = _appProvider.OwnerId,
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
            var app = _appRepository.ById(model.AppId);
            if (app == null) throw new Exception("App not found");
            CheckIsMine(app.Id);

            _templateRepository.Create(new Template
            {
               AppId = model.AppId,
               Name = model.Name,
               Custom = JsonConvert.SerializeObject(model.Custom),
               Html = model.Html,
               OwnerId = _appProvider.OwnerId
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
    }

    public class UpdateAutoEmailVM : EditCampaignVM
    {
        public int Id { get; set; } 
        public dynamic Custom { get; set; }
        public string TimeCondition { get; set; }
        public string TimeZone { get; set; }
    }

    public class CreateAutoEmailVM   : CreateCampaignVM
    {
        public int AutoResponderId { get; set; }
        public dynamic Custom { get; set; }
        public string TimeCondition { get; set; }
        public string TimeZone { get; set; }
    }

    public class UpdateAutoResponderVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public dynamic Custom { get; set; }
        public AutoResponderEnum Mode { get; set; }
    }

    public class CreateAutoResponderVM
    {
        public int ListId { get; set; }
        public string Name { get; set; }
        public dynamic Custom { get; set; }
        public AutoResponderEnum Mode { get; set; }
    }
}
