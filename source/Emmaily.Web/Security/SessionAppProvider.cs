using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using Emaily.Core.Abstraction.Persistence;
using Emaily.Core.Data;
using Emaily.Core.Data.Complex;
using Emaily.Core.DTO;
using Emaily.Services;
using Emaily.Web.Models;
using Microsoft.AspNet.Identity;

namespace Emaily.Web.Security
{
    public class EmailProvider : IEmailProvider
    {
        public Task<string> SendEmailAsync(EmailAddress sender, EmailAddress Receiver, string subject, string plainText, string htmlText,
            string queryString)
        {
            throw new System.NotImplementedException();
        }

        public string SendEmail(EmailAddress sender, EmailAddress Receiver, string subject, string plainText, string htmlText,
            string queryString)
        {
            throw new System.NotImplementedException();
        }

        public string SendEmail(EmailAddress sender, EmailAddress Receiver, MailNote note)
        {
            throw new System.NotImplementedException();
        }
    }
    public class CloudProvider : ICloudProvider
    {
        public void VerifyEmail(string email)
        {
            throw new System.NotImplementedException();
        }

        public string VerifyDomain(string domain)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveEmail(string email)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveDomain(string domain)
        {
            throw new System.NotImplementedException();
        }
        public CloudServiceInfo Info { get; }
    }
    public class SessionAppProvider : IAppProvider
    {
        private readonly IRepository<UserApps> _repository; 
        public SessionAppProvider(IRepository<UserApps> repository)
        {
            _repository = repository;
        }

        private int[] _apps;
        public int[] Apps
        {
            get {
                return _apps ?? (_apps = _repository.All.Where(x => x.UserId == OwnerId).Select(x => x.AppId).ToArray());
            }
        }

        public string OwnerId
        {
            get { return HttpContext.Current?.User?.Identity?.GetUserId(); }
        }
    }
}