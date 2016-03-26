using System.Linq;
using System.Net.Http;      
using System.Web.Http;
using System.Web.Http.OData;
using Emaily.Core.Abstraction.Services;
using Emaily.Core.DTO;

namespace Emaily.Web.Controllers.Api.v1
{
    /// <summary>
    ///  Manages system accounts.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1/attachment/{template}")]
    public class AttachmentController : BasicApiController
    {
        private readonly IEmailService _service;

        public AttachmentController(IEmailService service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns template information
        /// </summary>
        /// <param name="template">Template / Campaign Id</param>
        /// <returns></returns>
        [HttpGet, Route,EnableQuery]
        public IQueryable<AttachmentVM> GetAsOData(int template)
        {
            return _service.Attachments(template);
        }

        /// <summary>
        /// Deletes a template by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="template">Template / Campaign Id</param>
        /// <returns></returns>
        [HttpDelete, Route("{id}")]
        public HttpResponseMessage Delete([FromUri]int id, [FromUri]int template)
        {
            if (ModelState.IsValid)
            {
                var item = _service.DeleteAttachment(template, id);
                return Accepted(item);
            }
            return Bad(ModelState);
        }
    }
}
