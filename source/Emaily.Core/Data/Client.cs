using Emaily.Core.Abstraction;

namespace Emaily.Core.Data
{
    public class Client : CustomEntity
    {
        public string ApiKey { get; set; }
        public string OwnerId { get; set; }

        public bool IsOwner { get; set; }

        public App App { get; set; }
        public int AppId { get; set; }
    }
}