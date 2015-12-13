using System;
using Emaily.Core.Abstraction;

namespace Emaily.Core.Data
{
    public class Domain : CustomEntity
    {
        public DateTime? Verified { get; set; }
        public bool IsDisabledSignup { get; set; }
    }
}