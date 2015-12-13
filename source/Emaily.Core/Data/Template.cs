using Emaily.Core.Abstraction;

namespace Emaily.Core.Data
{
    public class Template : CustomEntity
    {
        public App App { get; set; }
        public int AppId { get; set; }

        public string Html { get; set; }
        public string OwnerId { get; set; }
    }
}