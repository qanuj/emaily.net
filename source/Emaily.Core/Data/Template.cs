using Emaily.Core.Abstraction;

namespace Emaily.Core.Data
{
    public class Template : CustomEntity
    {
        public App App { get; set; }
        public int AppId { get; set; }

        public string HtmlText { get; set; }
        public string PlainText { get; set; }
        public string QueryString { get; set; }
        public string OwnerId { get; set; }
    }
}