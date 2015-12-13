using System.Collections;
using System.Collections.Generic;
using Emaily.Core.Abstraction;

namespace Emaily.Core.Data
{
    public class Subscriber : CustomEntity
    {
        public string OwnerId { get; set; }
        public string Email { get; set; }

        public List List { get; set; }
        public int ListId { get; set; }

        public App App { get; set; }
        public int AppId { get; set; }

        public bool IsUnsubscribed { get; set; }
        public bool IsBounced { get; set; }
        public bool IsSoftBounce { get; set; }
        public bool IsComplaint { get; set; }
        public bool IsConfirmed { get; set; }

        public Campaign LastCampaign { get; set; }
        public int? LastCampaignId { get; set; }
        public string MessageID { get; set; }

        public AutoResponder LastAutoResponder { get; set; }
        public int? LastAutoResponderId { get; set; }

        public IList<Click> Clicks { get; set; }
        public IList<Queue> Queues { get; set; }
        public IList<CampaignResult> Results { get; set; }

    }
}