using System;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Emaily.Core.Abstraction
{
    public interface IEntity
    {
        int Id { get; set; }
        DateTime Created { get; set; }
        DateTime? Modified { get; set; }
    }

    public interface ISoftDelete
    {
        DateTime? Deleted { get; set; }
    }
}

