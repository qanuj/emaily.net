using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emaily.Core.DTO
{
    public class FileAccessInfo
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Id { get; set; }
    }
    public class AppVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FromEmail { get; set; }
        public int Quota { get; set; }
        public string Plan { get; set; }
        public int PlanId { get; set; }
        public int Used { get; set; }
    }
}
