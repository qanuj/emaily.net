namespace Emaily.Core.DTO
{
    public class CreateAutoEmailVM : CreateCampaignVM
    {
        public int AutoResponderId { get; set; }
        public dynamic Custom { get; set; }
        public string TimeCondition { get; set; }
        public string TimeZone { get; set; }
    }
}