namespace Emaily.Core.DTO
{
    public class UpdateTemplateVM  : EmailBody
    {
        public int Id { get; set; }            
    }

    public class EmailBody
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public dynamic Custom { get; set; }
        public string HtmlText { get; set; }
        public string PlainText { get; set; }
        public string QueryString { get; set; }
        public SenderViewModel Sender { get; set; }
    }
}