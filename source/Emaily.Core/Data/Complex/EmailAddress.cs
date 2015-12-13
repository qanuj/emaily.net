using System.ComponentModel.DataAnnotations.Schema;

namespace Emaily.Core.Data.Complex
{
    [ComplexType]
    public class EmailAddress
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ReplyTo { get; set; }
    }
}