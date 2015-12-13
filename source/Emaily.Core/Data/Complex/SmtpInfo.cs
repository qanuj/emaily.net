using System.ComponentModel.DataAnnotations.Schema;

namespace Emaily.Core.Data.Complex
{
    [ComplexType]
    public class SmtpInfo
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string Ssl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}