using Emaily.Core.Enumerations;

namespace Emaily.Core.DTO
{
    public class CreateAutoEmailVM
    {
        public string Name { get; set; }
        public int AppId { get; set; }
        public int ListId { get; set; }
        public AutoResponderEnum Mode { get; set; }
    }
}