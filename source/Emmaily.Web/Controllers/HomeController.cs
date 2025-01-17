﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Emaily.Core.Abstraction.Persistence;
using Emaily.Core.Abstraction.Services;
using Emaily.Core.Data;
using Emaily.Core.Data.Complex;
using Emaily.Core.DTO;
using Emaily.Core.Enumerations;
using Emaily.Web.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace Emaily.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _AdminRole = "Admin";
        private string _RootUser = "a@e10.in";
        private readonly ApplicationRoleManager _roleManager;
        private readonly ApplicationUserManager _userManager;
        private readonly IRepository<App> _appRepository;
        private readonly IRepository<UserApps> _userAppRepository;
        private readonly IRepository<Plan> _planRepository;
        private readonly IEmailService _emailService;
        public HomeController(ApplicationRoleManager roleManager, ApplicationUserManager userManager, IRepository<App> appRepository, IEmailService emailService, IRepository<Plan> planRepository, IRepository<UserApps> userAppRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _appRepository = appRepository;
            _emailService = emailService;
            _planRepository = planRepository;
            _userAppRepository = userAppRepository;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Setup()
        {
            if (!_roleManager.RoleExists(_AdminRole))
            {
                _roleManager.Create(new Role() {Name = _AdminRole, Read = ApiAccessEnum.All-1, Write = ApiAccessEnum.All-1});
            }
            var usr = await _userManager.FindByEmailAsync(_RootUser);
            if (usr != null && !await _userManager.IsInRoleAsync(usr.Id, _AdminRole))
            {
                await _userManager.AddToRoleAsync(usr.Id, _AdminRole);
            }
            if (!_planRepository.All.Any())
            {
                _planRepository.Create(new Plan
                {
                   Name = "Free",
                   AnnualPrice = 0,
                   Currency = CurrencyEnum.INR,
                   DeliveryFees = 0,
                   Description = "Free Plan",
                   DiscountedPrice = 0,
                   IsPayGo = false,
                   Icon = "user",
                   Price = 0,
                   Quota = 100,
                   Rate = 10 
                });
                _planRepository.SaveChanges();
            }
            if (usr != null && !_appRepository.All.Any())
            {
                _emailService.CreateApp(new CreateAppVM()
                {
                   Sender = new EmailAddress
                   {
                        Name = "Emaily Services",
                        Email = "marketing@emaily.xyz",
                        ReplyTo = "marketing@emaily.xyz"
                   },
                   Email = "marketing@emaily.xyz",
                   Currency = CurrencyEnum.INR,
                   Plan = "Free",
                   Company = "Emaily",
                   Logo = "",
                   Smtp = new SmtpInfo()
                });
            }

            if (usr != null && !_userAppRepository.All.Any(x => x.UserId == usr.Id))
            {
                _userAppRepository.Create(new UserApps
                {
                    App = _appRepository.All.FirstOrDefault(x=>x.Name=="Emaily"),
                    User = usr
                });
                _userAppRepository.SaveChanges();
            }


            return Json(1, JsonRequestBehavior.AllowGet);
        }
                              
    }
}