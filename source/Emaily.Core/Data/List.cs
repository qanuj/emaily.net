using System.Collections.Generic;
using Emaily.Core.Abstraction;
using Emaily.Core.Data.Complex;

namespace Emaily.Core.Data
{
    public class List : CustomEntity
    {
        public App App { get; set; }
        public int AppId { get; set; }

        public string OwnerId { get; set; }

        public bool IsOptIn { get; set; }
        public bool IsUnsubcribeAllList { get; set; }

        public int PreviousCount { get; set; }
        public bool IsProcessing { get; set; }
        public int TotalRecord { get; set; }

        public string ConfirmUrl { get; set; }
        public string SubscribedUrl { get; set; }
        public string UnSubscribedUrl { get; set; }
        public MailNote ThankYou { get; set; }
        public MailNote GoodBye { get; set; }
        public MailNote Confirmation { get; set; }

        public IList<CampaignList> Campaign { get; set; }
        public IList<AutoResponder> AutoResponders { get; set; }
        public IList<Subscriber> Subscribers { get; set; }

    }
}