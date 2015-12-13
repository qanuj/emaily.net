using System;
using System.Collections.Generic;
using Emaily.Core.Abstraction;
using Emaily.Core.Enumerations;

namespace Emaily.Core.Data
{
    public class Campaign : CampaignBase
    {
        public IList<CampaignList> Lists { get; set; }

        public Campaign()
        {
            this.Lists=new List<CampaignList>();
        }
    }
}