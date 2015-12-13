using Emaily.Core.Enumerations;

namespace Emaily.Core.DTO
{
    public class CreateAutoResponderVM
    {
        public int ListId { get; set; }
        public string Name { get; set; }
        public dynamic Custom { get; set; }
        public AutoResponderEnum Mode { get; set; }
    }
}