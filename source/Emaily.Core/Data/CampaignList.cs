using Emaily.Core.Abstraction;

namespace Emaily.Core.Data
{
    public class CampaignList : Entity
    {
        public List List { get; set; }
        public int ListId { get; set; }

        public Campaign Campaign { get; set; }
        public int CampaignId { get; set; }
    }
}