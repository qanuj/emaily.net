using System.Runtime.Remoting.Messaging;

namespace Emaily.Core.DTO
{
    public class CreateListVM
    {
        public int AppId { get; set; }
        public string Name { get; set; }
    }

    public class AttachmentVM  : CreateAttachmentVM
    {
        public int Id { get; set; }
    }

    public class CreateAttachmentVM 
    {
        public int TemplateId { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }
        public string Url { get; set; }
    }
}