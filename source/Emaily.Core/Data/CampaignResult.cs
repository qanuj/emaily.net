using System;
using Emaily.Core.Abstraction;

namespace Emaily.Core.Data
{
    public class CampaignResult : Entity
    {
        public Subscriber Subscriber { get; set; }
        public int SubscriberId { get; set; }

        public Campaign Campaign { get; set; }
        public int CampaignId { get; set; }

        public string Country { get; set; }
        public string UserAgent { get; set; }

        public DateTime? Opened { get; set; }
        public DateTime? Bounced { get; set; }
        public DateTime? MarkedSpam { get; set; }
        public bool IsSoftBounce { get; set; }
    }
}