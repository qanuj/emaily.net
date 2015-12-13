﻿using Emaily.Core.Abstraction;

namespace Emaily.Core.Data
{
    public class AutoEmail : CampaignBase
    {
        public AutoResponder AutoResponder { get; set; }
        public int AutoResponderId { get; set; }

        public string TimeCondition { get; set; }
        public string TimeZone { get; set; }
    }
}