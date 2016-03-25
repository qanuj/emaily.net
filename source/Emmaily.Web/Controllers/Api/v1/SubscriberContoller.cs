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
    [RoutePrefix("api/v1/subscriber/{list}")]
    public class SubscriberController : BasicApiController
    {
        private readonly IEmailService _service;

        public SubscriberController(IEmailService service)
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
        public PageResult<SubscriberVM> GetAsOData(int list, ODataQueryOptions<SubscriberVM> options)
        {
            return Page(_service.Subscribers(list), options);
        }

        /// <summary>
        /// PUT command to insert a template
        /// </summary>
        /// <param name="list"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut, Route]
        public HttpResponseMessage Put(int list, CreateSubscriberVM model)
        {
            if (ModelState.IsValid)
            {
                var item = _service.ImportSubscribers(model.Data, list);
                return Accepted(item);
            }
            return Bad(ModelState);
        }

        /// <summary>
        /// PUT command to insert a template
        /// </summary>
        /// <param name="list"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        [HttpGet, Route("import/{filename}")]
        public HttpResponseMessage GetImport(int list, string filename)
        {
            if (ModelState.IsValid)
            {
                var filePath = HostingEnvironment.MapPath(string.Format("~/App_Data/Uploads/{0}.csv", filename));
                if (File.Exists(filePath))
                {
                    var item = _service.ImportSubscribers(File.OpenText(filePath) , list);
                    return Accepted(item);
                }
                return Bad(new {error="File not found",filename= filePath });
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
        public HttpResponseMessage Post(int list, UpdateSubscriberVM model)
        {
            if (ModelState.IsValid)
            {
                var item = _service.UpdateSubscriber(model);
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
        public SubscriberVM Get([FromUri]int id, [FromUri]int list)
        {
            return _service.SubscriberById(list,id);
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
                var item = _service.DeleteSubscriber(list, id);
                return Accepted(item);
            }
            return Bad(ModelState);
        }
    }
}
