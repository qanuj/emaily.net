using Emaily.Core.Abstraction;

namespace Emaily.Core.Data
{
    public class CampaignResult : Entity
    {
        public Subscriber Subscriber { get; set; }
        public int SubscriberId { get; set; }

        public CampaignBase Campaign { get; set; }
        public int CampaignId { get; set; }

        public string Country { get; set; }
        public string UserAgent { get; set; }

        public bool IsOpened { get; set; }
        public bool IsBounced { get; set; }
        public bool IsMarkedSpam { get; set; }
    }
}