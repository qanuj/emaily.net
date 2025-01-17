﻿using System;
using System.Collections.Generic;
using Emaily.Core.Data;
using Emaily.Core.Data.Complex;
using Emaily.Core.Enumerations;

namespace Emaily.Core.Abstraction
{ 

    public abstract class Campaign :  Template
    {
        public string Label { get; set; }

        public CampaignStatusEnum Status { get; set; }

        public bool IsHtml { get; set; }

        public int? Recipients { get; set; }
        public int? Sents { get; set; }
        public int? Clicks { get; set; }
        public int? Opens { get; set; }

        public string Errors { get; set; }
        public DateTime? Started { get; set; }
        public DateTime? Future { get; set; }
        public string TimeZone { get; set; }


        public IList<CampaignResult> Results { get; set; }
        public IList<Link> Links { get; set; }
        public IList<Queue> Queues { get; set; }

        protected Campaign()
        {
            this.Results = new List<CampaignResult>();
            this.Links = new List<Link>();
            this.Queues = new List<Queue>();
        }

    }
}