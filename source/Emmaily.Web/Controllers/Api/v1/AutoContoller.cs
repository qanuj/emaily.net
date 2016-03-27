using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
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
    [RoutePrefix("api/v1/auto/{list}")]
    public class AutoController : BasicApiController
    {
        private readonly IEmailService _service;

        public AutoController(IEmailService service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns template information
        /// </summary>
        /// <param name="list">List Id</param>
        /// <param name="options"></param>
        /// <returns></returns>
        [HttpGet, Route]
        public PageResult<AutoResponderVM> GetAsOData(int list, ODataQueryOptions<AutoResponderVM> options)
        {
            return Page(_service.Auto(list), options);
        }

        /// <summary>
        /// PUT command to insert a template
        /// </summary>
        /// <param name="list"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut, Route]
        public HttpResponseMessage Put(int list, CreateAutoResponderVM model)
        {
            if (ModelState.IsValid)
            {
                var item = _service.CreateAutoResponder(model, list);
                return Accepted(item);
            }
            return Bad(ModelState);
        }

        /// <summary>
        /// POST command to update a template
        /// </summary>
        /// <param name="list"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route]
        public HttpResponseMessage Post(int list, UpdateAutoResponderVM model)
        {
            if (ModelState.IsValid)
            {
                var item = _service.UpdateAutoResponder(model, list);
                return Accepted(item);
            }
            return Bad(ModelState);
        }

        /// <summary>
        /// Returns a template by id
        /// </summary>
        /// <param name="list"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("{id}")]
        public AutoResponderVM Get([FromUri]int id, [FromUri]int list)
        {
            return _service.AutoResponderById(list,id);
        }

        /// <summary>
        /// Deletes a template by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpDelete, Route("{id}")]
        public HttpResponseMessage Delete([FromUri]int id, [FromUri]int list)
        {
            if (ModelState.IsValid)
            {
                var item = _service.DeleteAutoResponder(list, id);
                return Accepted(item);
            }
            return Bad(ModelState);
        }
    }
}
