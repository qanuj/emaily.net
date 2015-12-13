using System.Net.Mail;
using System.Threading.Tasks;
using Emaily.Core.Data.Complex;

namespace Emaily.Services
{
    public interface IEmailProvider
    {
        Task<string> SendEmailAsync(EmailAddress sender, EmailAddress Receiver, string subject, string plainText,
            string htmlText, string queryString);
        string SendEmail(EmailAddress sender, EmailAddress Receiver, string subject, string plainText,
            string htmlText, string queryString);
        string SendEmail(EmailAddress sender, EmailAddress Receiver, MailNote note);
    }
}