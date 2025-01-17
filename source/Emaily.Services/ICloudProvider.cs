using System.CodeDom;
using Emaily.Core.DTO;
using Emaily.Core.Enumerations;

namespace Emaily.Services
{
    public interface ICloudProvider
    {
        void VerifyEmail(string email);
        string VerifyDomain(string domain);  //returns token
        void RemoveEmail(string email);
        void RemoveDomain(string domain);
        CloudServiceInfo Info { get; }
    }

    public interface INotificationHub
    {
        void Notify(string userId, NotificationTypeEnum mode, dynamic message);
    }

    public interface IAppProvider
    {
        int[] Apps { get; }
        string OwnerId { get;}
    }
    
}