using System;

namespace Emaily.Core.DTO
{
    public class SendCampaignVM
    {
        public int CampaignId { get; set; }
        public int[] Lists { get; set; }
        public DateTime? Future { get; set; }
        public string TimeZone { get; set; }
    }
}