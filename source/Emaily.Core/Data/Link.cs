using System.Collections.Generic;
using Emaily.Core.Abstraction;

namespace Emaily.Core.Data
{
    public class Link : Entity
    {
        public CampaignBase Campaign { get; set; }
        public int? CampaignId { get; set; } 

        public string Url { get; set; }
        public IList<Click> Clicks { get; set; } 
    }
}