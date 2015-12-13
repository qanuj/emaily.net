using Emaily.Core.Abstraction;

namespace Emaily.Core.Data
{
    public class Queue : Entity
    {
        public CampaignBase Campaign { get; set; }
        public int? CampaignId { get; set; }

        public Subscriber Subscriber { get; set; }
        public int? SubscriberId { get; set; }

        public string QueryString { get; set; }

        public bool IsSent { get; set; }
    }
}