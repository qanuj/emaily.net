using System;
using System.Collections.Generic;
using Emaily.Core.Abstraction;

namespace Emaily.Core.Data
{
    public class Promo : Entity
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int? Quota { get; set; }
        
        public int? Discount { get; set; }
        public bool IsDiscountPercentage { get; set; } 
        public int DiscountApplicationMonths { get; set; }//-1 for forever.

        public IList<Plan> Plans { get; set; }

        public Promo()
        {
            this.Plans=new List<Plan>();
        }
    }
}