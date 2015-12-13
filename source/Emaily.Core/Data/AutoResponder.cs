using System.Collections;
using System.Collections.Generic;
using Emaily.Core.Abstraction;
using Emaily.Core.Enumerations;

namespace Emaily.Core.Data
{
    public class AutoResponder : CustomEntity
    {
        public AutoResponderEnum  Mode { get; set; }

        public List List { get; set; }
        public int ListId { get; set; }

        public IList<AutoEmail> Emails { get; set; }
    }
}