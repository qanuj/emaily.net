using System;
using System.Linq;
using Emaily.Core.Abstraction.Persistence;
using Emaily.Core.Abstraction.Services;
using Emaily.Core.Data;
using Emaily.Core.DTO;
using System.Collections.Generic;
using Emaily.Core.Data.Complex;
using Emaily.Core.Enumerations;

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

    public class SendCampaignVM
    {
        public int CampaignId { get; set; }
        public int[] Lists { get; set; }
        public DateTime? Future { get; set; }
        public string Timezone { get; set; }
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
        private readonly IRepository<Client> _clientRepository;
        private readonly IRepository<Campaign> _campaignRepository;
        private readonly IRepository<CampaignList> _campaignListRepository;
        private readonly IRepository<CampaignResult> _campaignResultRepository;
        private readonly IRepository<Link> _linkRepository;
        private readonly IRepository<Subscriber> _subscriberRepository;
        private readonly IRepository<List> _listRepository;
        private readonly IRepository<Promo> _promoRepository;
        private readonly IEmailProvider _emailProvider;
        private readonly IStorageProvider _storageProvider;


        public EmailService(IRepository<App> appRepository, IRepository<Plan> planRepository, IRepository<Client> clientRepository, IRepository<Campaign> campaignRepository, IRepository<List> listRepository, IRepository<AutoEmail> autoEmailRepository, IRepository<AutoResponder> autoResponderRepository, IRepository<Domain> domainRepository, IRepository<Queue> queueRepository, IRepository<Template> templateRepository, IRepository<CampaignList> campaignListRepository, IRepository<CampaignResult> campaignResultRepository, IRepository<Link> linkRepository, IRepository<Subscriber> subscriberRepository, IRepository<Promo> promoRepository, IEmailProvider emailProvider, IStorageProvider storageProvider)
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
        }

        private IQueryable<AppVM> Apps()
        {
            return _appRepository.All.Select(x => new AppVM
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

        private IQueryable<CampaignVM> Campaigns()
        {
            return _campaignRepository.All.Select(x => new CampaignVM
            {
                Errors = x.Errors,
                Id = x.Id,
                Title = x.Label=="" || x.Label==null ?  x.Name : x.Label,
                Status = x.Status,
                Recipients = x.Recipients,
                Started = x.Started,
                UniqueClicks = x.Clicks,
                UniqueOpens = x.Opens
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

        public AppVM UpdateApp(UpdateAppVM model)
        {
            var app = _appRepository.ById(model.Id);
            if (app == null) throw new Exception("App not found");

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

            _emailProvider.VerifyEmail(model.Sender.Email);

            _appRepository.SaveChanges();

            return Apps().FirstOrDefault(x => x.Id == app.Id);
        }
    }
}
