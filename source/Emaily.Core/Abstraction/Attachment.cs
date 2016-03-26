using Emaily.Core.Data;

namespace Emaily.Core.Abstraction
{
    public class Attachment : Entity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }

        public Template Template { get; set; }
        public int TemplateId { get; set; }
    }
}