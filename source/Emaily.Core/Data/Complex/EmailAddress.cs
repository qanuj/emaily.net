using System.ComponentModel.DataAnnotations.Schema;

namespace Emaily.Core.Data.Complex
{
    [ComplexType]
    public class EmailAddress
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ReplyTo { get; set; }

        public EmailAddress() { }
        public EmailAddress(string email,string name)
        {
            this.Email = email;
            this.Name = name;
        }
    }
}