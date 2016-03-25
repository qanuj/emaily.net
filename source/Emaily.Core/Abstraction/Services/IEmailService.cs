using System.Collections.Generic;
using System.Linq;
using Emaily.Core.DTO;

namespace Emaily.Core.Abstraction.Services
{
    public interface IEmailService
    {
        ListVM CreateList(CreateListVM model);
        void Subscribe(CreateSubscriber model);
        ImportResult ImportSubscribers(IDictionary<string, ListEmail> items, int listId);
        ImportResult ImportSubscribers(string importData, int listId);
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

        void CreateAutoResponder(CreateAutoResponderVM model);
        void UpdateAutoResponder(UpdateAutoResponderVM model);

        void AddCustomField(CustomFieldVM model);
        void RenameCustomField(RenameCustomFieldVM model);
        void DeleteCustomField(CustomFieldVM model);

        IQueryable<CampaignVM> Campaigns();
        IQueryable<TemplateVM> Templates();
        IQueryable<ListVM> Lists();
        IQueryable<SubscriberVM> Subscribers(int listId);
        IQueryable<AppVM> Apps();
        TemplateInfoVM TemplateById(int id);
        bool DeleteTemplate(int id);
        ListInfoVM ListById(int id);
        bool DeleteList(int id);
        IQueryable<CampaignReportVM> CampaignReports();
        SubscriberVM UpdateSubscriber(UpdateSubscriberVM model);
        SubscriberVM SubscriberById(int list, int id);
        bool DeleteSubscriber(int list, int id);
    }
}
