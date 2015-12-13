using Emaily.Core.Data;

namespace Emaily.Core.Abstraction
{
    public abstract class CustomEntity : Entity
    {
        public string Name { get; set; }
        public string Custom { get; set; }
    }
}