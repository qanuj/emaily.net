using System.Collections.Generic;
using Emaily.Core.Abstraction;
using Emaily.Core.Enumerations;
using Microsoft.SqlServer.Server;

namespace Emaily.Core.Data
{
    public class Plan : CustomEntity
    {
        public string Icon { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        public bool IsPayGo { get; set; }
        public int DeliveryFees { get; set; }
        public int Rate { get; set; }

        public int Quota { get; set; }
        public int? AnnualPrice { get; set; }
        public int? DiscountedPrice { get; set; }

        public Promo Promo { get; set; }
        public int PromoId { get; set; }

        public CurrencyEnum Currency { get; set; }

        public IList<App> Apps { get; set; }

        public Plan()
        {
            this.Apps=new List<App>();
        }
    }
}