using System.ComponentModel.DataAnnotations.Schema;

namespace Emaily.Core.Data.Complex
{
    [ComplexType]
    public class MailNote
    {
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}