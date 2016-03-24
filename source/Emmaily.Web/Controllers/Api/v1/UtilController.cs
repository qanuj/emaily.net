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
        public UtilController(IUtilService util)
        {
            _util = util;
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
    }
}
