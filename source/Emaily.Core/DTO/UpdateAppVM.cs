using Emaily.Core.Data.Complex;

namespace Emaily.Core.DTO
{
    public class UpdateAppVM
    {
        public string Company { get; set; }
        public SmtpInfo Smtp { get; set; }
        public string FromName { get; set; }
        public string ReplyTo { get; set; }
        public string Logo { get; set; }
        public int Id { get; set; }
    }
}