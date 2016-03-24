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
    [RoutePrefix("api/v1/list")]
    public class ListController : BasicApiController
    {
        private readonly IEmailService _service;

        public ListController(IEmailService service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns template information
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [HttpGet, Route]
        public PageResult<ListVM> GetAsOData(ODataQueryOptions<ListVM> options)
        {
            return Page(_service.Lists(), options);
        }
        
        /// <summary>
        /// Returns all template information
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("all"), EnableQuery]
        public IQueryable<ListVM> GetAll()
        {
            return _service.Lists();
        }

        /// <summary>
        /// PUT command to insert a template
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut, Route]
        public HttpResponseMessage Put(CreateListVM model)
        {
            if (ModelState.IsValid)
            {
                var item = _service.CreateList(model); 
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
        public HttpResponseMessage Post(UpdateListVM model)
        {
            if (ModelState.IsValid)
            {
                var item = _service.UpdateList(model);
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
        public ListInfoVM Get([FromUri]int id)
        {
            return _service.ListById(id);
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
                var item = _service.DeleteList(id);
                return Accepted(item);
            }
            return Bad(ModelState);
        }
    }
}
