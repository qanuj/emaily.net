﻿using System.Collections;
using Emaily.Core.Abstraction;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Emaily.Core.Data.Complex;
using Emaily.Core.Enumerations;

namespace Emaily.Core.Data
{
    public class App : CustomEntity
    {                
        public CurrencyEnum Currency { get; set; }
        public SmtpInfo Smtp { get; set; }
        public EmailAddress Sender { get; set; }

        public bool IsBounceSetup { get; set; }
        public bool IsComplaintSetup { get; set; }

        public string AppKey { get; set; }
        public string TestEmail { get; set; }
        public string Logo { get; set; }
        public int Quota { get; set; }
        public int Used { get; set; }

        public Plan Plan { get; set; }
        public int PlanId { get; set; }

        public Promo Promo { get; set; }
        public int? PromoId { get; set; }

        public IList<Client> Clients { get; set; }
        public IList<List> Lists { get; set; }
        public IList<Subscriber> Subscribers { get; set; }
        public IList<Template> Templates { get; set; }

        public App()
        {
            this.Smtp=new SmtpInfo();
            this.Sender=new EmailAddress();
            this.Clients=new List<Client>();
            this.Lists=new List<List>();
            this.Subscribers = new List<Subscriber>();
            this.Templates = new List<Template>();
        }
    }
}