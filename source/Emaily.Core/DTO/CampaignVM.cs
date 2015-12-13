using System;
using Emaily.Core.Enumerations;

namespace Emaily.Core.DTO
{
    public class CampaignVM
    {
        public int Id { get; set; }
        public string Errors { get; set; }
        public string Title { get; set; }
        public int Recipients { get; set; }
        public DateTime? Started { get; set; }
        public int UniqueOpens { get; set; }
        public int UniqueClicks { get; set; }
        public CampaignStatusEnum Status { get; set; }
    }
}