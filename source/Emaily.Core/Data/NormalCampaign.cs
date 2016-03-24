﻿using System;
using System.Collections.Generic;
using Emaily.Core.Abstraction;
using Emaily.Core.Enumerations;

namespace Emaily.Core.Data
{
    public class NormalCampaign : Campaign
    {
        public IList<CampaignList> Lists { get; set; }

        public NormalCampaign()
        {
            this.Lists=new List<CampaignList>();
        }
    }
}