using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emaily.Core.DTO
{
    public class CloudServiceInfo
    {
        public string Region { get; set; }
        public double MaxSend { get; set; }
        public double Rate { get; set; }
        public double Sent { get; set; }
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
