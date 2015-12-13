namespace Emaily.Core.DTO
{
    public class EditCampaignVM
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string HtmlText { get; set; }
        public string PlainText { get; set; }
        public bool IsHtml { get; set; }
        public string FromName { get; set; }
        public string ReplyTo { get; set; }
        public string QueryString { get; set; }
        public string Label { get; set; }
        public int CampaignId { get; set; }
    }
}