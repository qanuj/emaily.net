using System;

namespace Emaily.Core.Enumerations
{
    [Flags]
    public enum ApiAccessEnum
    {
        None = 0,
        Campaign = 1,
        Template = 2,
        List = 4,
        Report = 8,
        All = 16
    }
}