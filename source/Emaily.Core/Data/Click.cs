using Emaily.Core.Abstraction;

namespace Emaily.Core.Data
{
    public class Click : Entity
    {
        public Subscriber Subscriber { get; set; }
        public int SubscriberId { get; set; }

        public Link Link { get; set; }
        public int LinkId { get; set; }

    }
}