using System;

namespace Emaily.Data
{
    public interface IEntityFunctions
    {
        int? DiffDays2(DateTime? date1, DateTime? date2);
    }
}