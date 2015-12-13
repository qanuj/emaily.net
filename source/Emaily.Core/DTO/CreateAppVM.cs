using Emaily.Core.Data.Complex;
using Emaily.Core.Enumerations;

namespace Emaily.Core.DTO
{
    public class CreateAppVM
    {
        public string Company { get; set; }
        public string Email { get; set; }
        public string Plan { get; set; }
        public CurrencyEnum Currency { get; set; }
        public SmtpInfo Smtp { get; set; }
        public EmailAddress Sender { get; set; }
        public string Logo { get; set; }
        public string PromoCode { get; set; }
    }
}