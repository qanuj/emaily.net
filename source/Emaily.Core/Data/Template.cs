using System.Collections.Generic;
using Emaily.Core.Abstraction;
using Emaily.Core.Data.Complex;
using Emaily.Core.DTO;

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
        public EmailAddress Sender { get; set; }

        public int Price { get; set; }

        public ICollection<Attachment> Attachments { get; set; }

        public Template()
        {
            Sender=new EmailAddress();
        }
    }
}