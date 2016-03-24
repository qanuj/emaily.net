using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Emaily.Core.Abstraction.Services;
using Emaily.Core.DTO;

namespace Emaily.Web.Controllers.Api.v1
{
    [RoutePrefix("api/v1/util")]
    public class UtilController : BasicApiController
    {
        private readonly IUtilService _util;
        private readonly ApplicationUserManager _userManager;
        public UtilController(IUtilService util, ApplicationUserManager userManager)
        {
            _util = util;
            _userManager = userManager;
        }

        [HttpGet, Route("enums")]
        public EnumList GetAllEnums()
        {
            return _util.Enums();
        }
            
        [HttpGet, Route("base")]
        public string GetBase()
        {
            return Request.RequestUri.Scheme + System.Uri.SchemeDelimiter + HttpContext.Current.Request.Url.Authority;
        }

        [HttpGet, Route("me")]
        public async Task<HttpResponseMessage> GetSelf(string apiKey)
        {
            var me = await _userManager.FindByNameAsync(User.Identity.Name);
            if (me == null) return Bad("Unauthorised");
            return Ok(new
            {
                me.Email,
                Picture = me.Profile != null ? me.Profile.Picture : "",
                Name = me.Profile != null ? me.Profile.Name : me.Email
            });
        }
    }
}
