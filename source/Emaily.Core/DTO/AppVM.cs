using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emaily.Core.Data.Complex;
using Emaily.Core.Enumerations;

namespace Emaily.Core.DTO
{
    public class CloudServiceInfo
    {
        public string Region { get; set; }
        public double MaxSend { get; set; }
        public double Rate { get; set; }
        public double Sent { get; set; }
    }
    public class AppVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FromEmail { get; set; }
        public int Quota { get; set; }
        public string Plan { get; set; }
        public int PlanId { get; set; }
        public int Used { get; set; }
    }

    public class CreateAppVM
    {
        public string Company { get; set; }
        public string Email { get; set; }
        public string Plan { get; set; }
        public CurrencyEnum Currency { get; set; }
        public SmtpInfo Smtp { get; set; }
        public EmailAddress Sender { get; set; }
        public string Logo { get; set; }
        public string PromoCode { get; set; }
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
    }

    public class ListEmail
    {
        public int ListId { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class UpdateSubscriptionVM
    {
        public int Id { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Token { get; set; }
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
    }

    public class CampaignResultVM
    {
        public int CampaignId { get; set; }
        public int SubscriberId { get; set; }
    }

    public class CreateClickVM : CampaignResultVM
    {
        public int LinkId { get; set; }
    }
}
