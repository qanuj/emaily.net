namespace Emaily.Core.DTO
{
    public class UpdateAutoEmailVM : EditCampaignVM
    {                        
        public string TimeCondition { get; set; }
        public string TimeZone { get; set; }
    }
}