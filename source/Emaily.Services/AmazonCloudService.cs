using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Emaily.Core.Data.Complex;
using Emaily.Core.DTO;

namespace Emaily.Services
{
    public interface ICloudConfigProvider
    {
        
    }

    public interface IAmazonConfigProvider : ICloudConfigProvider
    {
        string Region { get; }
        string AccessKey { get;}
        string Secret { get;}
        string BounceTopic { get;}
        string ComplaintTopic { get;}
    }

    public class AmazonCloudService : ICloudProvider, IEmailProvider
    {
        private readonly AmazonSimpleEmailServiceClient _amazon;
        private readonly IAmazonConfigProvider _config;
        public AmazonCloudService(IAmazonConfigProvider config)
        {
            _config = config;
            _amazon = new AmazonSimpleEmailServiceClient(new BasicAWSCredentials(config.AccessKey,config.Secret), new AmazonSimpleEmailServiceConfig
            {
                AuthenticationRegion = config.Region
            });
        }

        public async Task<string> SendEmailAsync(EmailAddress sender, EmailAddress Receiver, string subject, string plainText, string htmlText, string queryString)
        {
            var body = new Body { Html = new Content(htmlText), Text = new Content(plainText) };
            var task = await _amazon.SendEmailAsync(new SendEmailRequest
            {
                Destination = new Destination(new List<string> { sender.Email }),
                Message = new Message(new Content(subject), body),
                ReplyToAddresses = new List<string> { sender.Email },
                Source = sender.Email
            });

            return task.MessageId;
        }

        public string SendEmail(EmailAddress sender, EmailAddress Receiver, string subject, string plainText, string htmlText, string queryString)
        {
            var body = new Body { Html = new Content(htmlText), Text = new Content(plainText) };
            var task = _amazon.SendEmail(new SendEmailRequest
            {
                Destination = new Destination(new List<string> { sender.Email }),
                Message = new Message(new Content(subject), body),
                ReplyToAddresses = new List<string> { sender.Email },
                Source = sender.Email
            });
            return task.MessageId;
        }

        public string SendEmail(EmailAddress sender, EmailAddress Receiver, MailNote note)
        {
            var body = new Body { Html = new Content(note.Message) };
            var task = _amazon.SendEmail(new SendEmailRequest
            {
                Destination = new Destination(new List<string> { sender.Email }),
                Message = new Message(new Content(note.Subject), body),
                ReplyToAddresses = new List<string> { sender.Email },
                Source = sender.Email
            });
            return task.MessageId;
        }

        public void VerifyEmail(string email)
        {
            var result=_amazon.VerifyEmailAddress(new VerifyEmailAddressRequest()
            {
                EmailAddress = email
            });
        }

        public string VerifyDomain(string domain)
        {
            domain = domain.ToLower();
            var query = _amazon.ListIdentities(new ListIdentitiesRequest {IdentityType = IdentityType.Domain});
            if (query.Identities.Contains(domain)) throw new Exception(string.Format("Domain '{0}' already added",domain));

            var result = _amazon.VerifyDomainIdentity(new VerifyDomainIdentityRequest
            {
                Domain = domain
            });
            AddTextRecord(domain, result.VerificationToken, "_amazonses");
            var dkim=_amazon.VerifyDomainDkim(new VerifyDomainDkimRequest {Domain = domain});
            if (dkim.DkimTokens.Count > 0)
            {
                AddDkims(domain, dkim.DkimTokens);
            }
            _amazon.SetIdentityNotificationTopic(new SetIdentityNotificationTopicRequest
            {
                Identity = domain,
                NotificationType = NotificationType.Bounce,
                SnsTopic = _config.BounceTopic
            });
            _amazon.SetIdentityNotificationTopic(new SetIdentityNotificationTopicRequest
            {
                Identity = domain,
                NotificationType = NotificationType.Complaint,
                SnsTopic = _config.ComplaintTopic
            });
            _amazon.SetIdentityDkimEnabled(new SetIdentityDkimEnabledRequest
            {
                Identity = domain,
                DkimEnabled = true
            });
            return string.Empty;
        }

        private void AddTextRecord(string domain, string verificationToken, string amazonses)
        {
            throw new System.NotImplementedException();
        }

        private void AddDkims(string domain, List<string> dkimTokens)
        {
            //throw new System.NotImplementedException();
        }


        public void RemoveEmail(string email)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveDomain(string domain)
        {
            _amazon.DeleteIdentity(new DeleteIdentityRequest
            {
                //Identity =  
            });
        }

        private CloudServiceInfo _info;
        public CloudServiceInfo Info
        {
            get
            {
                if (_info == null)
                {
                    var quota=_amazon.GetSendQuota(new GetSendQuotaRequest());

                    _info = new CloudServiceInfo
                    {
                        Region = _amazon.Config.AuthenticationRegion,
                        MaxSend = quota.Max24HourSend,
                        Rate = quota.MaxSendRate,
                        Sent = quota.SentLast24Hours
                    };
                }
                return _info;
            }
        }
    }
}