using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Emaily.Core.Data.Complex;

namespace Emaily.Services
{
    public class AmazonCloudService : ICloudProvider, IEmailProvider
    {
        private readonly AmazonSimpleEmailServiceClient _amazon;
        public AmazonCloudService(AmazonSimpleEmailServiceClient amazon)
        {
            _amazon = amazon;
        }

        public Task SendEmailAsync(EmailAddress sender, EmailAddress Receiver, string subject, string plainText, string htmlText,string queryString)
        {
            var body = new Body() { Html=new Content(htmlText), Text = new Content(plainText)};
            var task= _amazon.SendEmailAsync(new SendEmailRequest
            {
                Destination = new Destination(new List<string> { sender.Email }),
                Message = new Message(new Content(subject), body),
                ReplyToAddresses = new List<string> { sender.Email },
                Source = sender.Email
            });

            return task;
        }

        public void VerifyEmail(string email)
        {
            _amazon.VerifyEmailAddress(new VerifyEmailAddressRequest()
            {
                EmailAddress = email
            });
        }
    }
}