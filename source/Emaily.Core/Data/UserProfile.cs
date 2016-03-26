using System;
using Emaily.Core.Abstraction;

namespace Emaily.Core.Data
{
    public class UserProfile : CustomEntity
    {
        public string Picture { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public DateTime? Birthday { get; set; }
    }
}