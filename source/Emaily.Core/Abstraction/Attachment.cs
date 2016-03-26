using Emaily.Core.Data;

namespace Emaily.Core.Abstraction
{
    public class Attachment : Entity
    {
        public string FileName { get; set; }
        public string Url { get; set; }

        public Template Template { get; set; }
        public int TemplateId { get; set; }
    }
}