using System.CodeDom;
using Emaily.Core.DTO;

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


    public interface IAppProvider
    {
        int[] Apps { get; }
        string OwnerId { get;}
    }
    
}