using Emaily.Core.Enumerations;

namespace Emaily.Core.DTO
{
    public class UpdateAutoResponderVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public dynamic Custom { get; set; }
        public AutoResponderEnum Mode { get; set; }
    }
}