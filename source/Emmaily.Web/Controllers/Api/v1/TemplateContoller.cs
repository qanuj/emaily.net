using System.Linq;
using System.Net.Http;
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
    [RoutePrefix("api/v1/template")]
    public class TemplateController : BasicApiController
    {
        private readonly IEmailService _service;

        public TemplateController(IEmailService service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns template information
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [HttpGet, Route]
        public PageResult<TemplateVM> GetAsOData(ODataQueryOptions<TemplateVM> options)
        {
            return Page(_service.Templates(), options);
        }
        
        /// <summary>
        /// Returns all template information
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("all"), EnableQuery]
        public IQueryable<TemplateVM> GetAll()
        {
            return _service.Templates();
        }

        /// <summary>
        /// PUT command to insert a template
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut, Route]
        public HttpResponseMessage Put(CreateTemplateVM model)
        {
            if (ModelState.IsValid)
            {
                var item = _service.CreateTemplate(model); 
                return Accepted(item);
            }
            return Bad(ModelState);
        }

        /// <summary>
        /// POST command to update a template
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route]
        public HttpResponseMessage Post(UpdateTemplateVM model)
        {
            if (ModelState.IsValid)
            {
                var item = _service.UpdateTemplate(model);
                return Accepted(item);
            }
            return Bad(ModelState);
        }

        /// <summary>
        /// Returns a template by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("{id}")]
        public TemplateInfoVM Get([FromUri]int id)
        {
            return _service.TemplateById(id);
        }

        /// <summary>
        /// Deletes a template by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("{id}")]
        public HttpResponseMessage Delete([FromUri]int id)
        {
            if (ModelState.IsValid)
            {
                var item = _service.DeleteTemplate(id);
                return Accepted(item);
            }
            return Bad(ModelState);
        }
    }
}
