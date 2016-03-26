namespace Emaily.Core.DTO
{
    public class EditCampaignVM  : EmailBody
    {
        public bool IsHtml { get; set; }
        public int Id { get; set; }
        public int[] Lists { get; set; }
    }
}