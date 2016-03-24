namespace Emaily.Core.DTO
{
    public class UpdateTemplateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public dynamic Custom { get; set; }
        public string HtmlText { get; set; }
        public string PlainText { get; set; }
        public string QueryString { get; set; }
    }
}