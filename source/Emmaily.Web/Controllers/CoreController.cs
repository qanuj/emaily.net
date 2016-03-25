using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Emaily.Web.Controllers
{
    public class CoreController : Controller
    {
        protected ActionResult Json2(object obj)
        {
            return Content(JsonConvert.SerializeObject(obj, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }), "application/json");
        }
    }
}