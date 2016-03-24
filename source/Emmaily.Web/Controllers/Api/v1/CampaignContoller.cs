using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Emaily.Core.Abstraction.Services;
using Emaily.Core.DTO;

namespace Emaily.Web.Controllers.Api.v1
{
    /// <summary>
    ///  Manages system accounts.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/campaign")]
    public class CampaignController : BasicApiController
    {
        private readonly IEmailService _service;

        public CampaignController(IEmailService service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns campaign information
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [HttpGet, Route]
        public PageResult<CampaignVM> GetAsOData(ODataQueryOptions<CampaignVM> options)
        {
            return Page(_service.Campaigns(), options);
        }
        
        /// <summary>
        /// Returns all campaign information
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("all"), EnableQuery]
        public IQueryable<CampaignVM> GetAll()
        {
            return _service.Campaigns();
        }

        /// <summary>
        /// PUT command to insert a campaign
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut, Route]
        public HttpResponseMessage Put(CreateCampaignVM model)
        {
            if (ModelState.IsValid)
            {
                var item = _service.CreateCampaign(model); 
                return Accepted(item);
            }
            return Bad(ModelState);
        }

        /// <summary>
        /// POST command to update a campaign
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route]
        public HttpResponseMessage Post(EditCampaignVM model)
        {
            if (ModelState.IsValid)
            {
                var item = _service.UpdateCampaign(model);
                return Accepted(item);
            }
            return Bad(ModelState);
        }

        /// <summary>
        /// Returns a campaign by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("{id}")]
        public CampaignInfoVM Get([FromUri]int id)
        {
            return _service.CampaignById(id);
        }

        /// <summary>
        /// Deletes a campaign by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("{id}")]
        public HttpResponseMessage Delete([FromUri]int id)
        {
            if (ModelState.IsValid)
            {
                var item = _service.DeleteCampaign(id);
                return Accepted(item);
            }
            return Bad(ModelState);
        }
    }
}
