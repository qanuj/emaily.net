using System.Collections.Generic;
using System.IO;
using System.Linq;
using Emaily.Core.DTO;
using System.Threading.Tasks;

namespace Emaily.Core.Abstraction.Services
{
    public interface IEmailService
    {
        ListVM CreateList(CreateListVM model);
        SubscriberVM Subscribe(CreateSubscriber model);
        Task ImportSubscribers(string importData, int listId);
        Task ImportSubscribers(TextReader reader, int listId);

        void ConfirmSubscription(UpdateSubscriptionVM model);
        void Unsubscribe(ListEmail model);
        ListVM RenameList(RenameListVM model);
        ListVM UpdateList(UpdateListVM model);
        AppVM UpdateApp(UpdateAppVM model);
        void SendCampaign(SendCampaignVM model);

        CampaignVM CreateCampaign(CreateCampaignVM model);
        CampaignVM UpdateCampaign(EditCampaignVM model);

        bool DeleteCampaign(int id);
        CampaignInfoVM CampaignById(int id);

        AppVM CreateApp(CreateAppVM model);
        void MarkRead(CampaignResultVM model, string country, string userAgent);
        void MarkSpam(CampaignResultVM model, string country, string userAgent);
        void MarkBounced(CampaignResultVM model, string country, string userAgent, bool IsSoftBounce);
        void CreateOrUpdateClick(CreateClickVM model, string country, string userAgent);

        TemplateVM CreateTemplate(CreateTemplateVM model);
        TemplateVM UpdateTemplate(UpdateTemplateVM model);

        void CreateAutoEmail(CreateAutoEmailVM model);
        void UpdateAutoEmail(UpdateAutoEmailVM model);

        AutoResponderVM CreateAutoResponder(CreateAutoResponderVM model,int list);
        AutoResponderVM UpdateAutoResponder(UpdateAutoResponderVM model, int list);

        void AddCustomField(CustomFieldVM model);
        void RenameCustomField(RenameCustomFieldVM model);
        void DeleteCustomField(CustomFieldVM model);

        IQueryable<CampaignVM> Campaigns();
        IQueryable<TemplateVM> Templates();
        IQueryable<ListVM> Lists();
        IQueryable<SubscriberVM> Subscribers(int listId);
        IQueryable<SubscriberReportVM> SubscriberReport(int listId);
        IQueryable<AutoResponderVM> Auto(int listId);
        IQueryable<AppVM> Apps();
        TemplateInfoVM TemplateById(int id);
        bool DeleteTemplate(int id);
        ListInfoVM ListById(int id);
        bool DeleteList(int id);
        IQueryable<CampaignReportVM> CampaignReports();
        SubscriberVM UpdateSubscriber(UpdateSubscriberVM model);
        SubscriberVM SubscriberById(int list, int id);
        bool DeleteSubscriber(int list, int id);

        IQueryable<AttachmentVM> Attachments(int template);
        AttachmentVM CreateAttachment(CreateAttachmentVM model, int template);
        bool DeleteAttachment(int template, int id,bool deleteFile,string folder);
        bool DeleteAutoResponder(int list, int id);
        AutoResponderVM AutoResponderById(int list, int id);
        SubscriberCountReportVM SubscriberCountReport(int list);
    }
}
