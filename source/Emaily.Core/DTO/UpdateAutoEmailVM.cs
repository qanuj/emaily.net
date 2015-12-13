namespace Emaily.Core.DTO
{
    public class UpdateAutoEmailVM : EditCampaignVM
    {
        public int Id { get; set; }
        public dynamic Custom { get; set; }
        public string TimeCondition { get; set; }
        public string TimeZone { get; set; }
    }
}