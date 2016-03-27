using System;
using System.Collections.Generic;
using Emaily.Core.Abstraction;
using Emaily.Core.Data.Complex;

namespace Emaily.Core.Data
{
    public class SubscriberReport : Entity
    {
        public int Total { get; set; }
        public List List { get; set; }
        public int ListId { get; set; }
        public SubscriberReport() { }
        public SubscriberReport(int total,int listId)
        {
            this.Total = total;
            this.ListId = listId;
        }
    }
    public class List : CustomEntity
    {
        public string Key { get; set; }

        public App App { get; set; }
        public int AppId { get; set; }

        public string OwnerId { get; set; }

        public bool IsUnsubcribeAllList { get; set; }

        public int PreviousCount { get; set; }
        public bool IsProcessing { get; set; }
        public int TotalRecord { get; set; }

        public MailNote ThankYou { get; set; }
        public MailNote GoodBye { get; set; }
        public MailNote Confirmation { get; set; }

        public IList<CampaignList> Campaigns { get; set; }
        public IList<SubscriberReport> Reports { get; set; }
        public IList<AutoResponder> AutoResponders { get; set; }
        public IList<Subscriber> Subscribers { get; set; }

        public List()
        {
            this.ThankYou=new MailNote();
            this.GoodBye=new MailNote();
            this.Confirmation=new MailNote();
            this.Campaigns=new List<CampaignList>();
            this.AutoResponders=new List<AutoResponder>();
            this.Subscribers=new List<Subscriber>();
        }
    }
}