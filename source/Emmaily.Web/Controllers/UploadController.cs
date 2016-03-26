using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Web.Hosting;
using System.Web.Mvc;
using Emaily.Core.Abstraction.Services;
using Emaily.Core.DTO;
using Emaily.Web.Utils;

namespace Emaily.Web.Controllers
{
    [Authorize]
    public class UploadController : CoreController
    {
        private readonly IEmailService _service;
        private readonly string[] Documents = { ".png", ".jpeg", "jpg", ".docx", ".pdf", ".xls", ".xlsx" };
        private readonly string[] Pictures = { ".png", ".jpeg", "jpg" };
        private readonly string[] CSV = { ".csv" };
        public UploadController(IEmailService service)
        {
            _service = service;
            _storageRoot = FindStorageRoot();
        }

        private static string _storageRoot;
        public static string FindStorageRoot()
        {
            if (!string.IsNullOrWhiteSpace(_storageRoot)) return _storageRoot;
            var folder = "~/App_Data/Uploads";
            if (ConfigurationManager.AppSettings.AllKeys.Any(x => x == "StorageRoot"))
            {
                var tmp = ConfigurationManager.AppSettings["StorageRoot"];
                if (!string.IsNullOrWhiteSpace(tmp)) folder = tmp;
            }
            var storageRoot = HostingEnvironment.MapPath(folder);
            if (!string.IsNullOrWhiteSpace(storageRoot) && !Directory.Exists(storageRoot))
                Directory.CreateDirectory(storageRoot);
            return storageRoot;
        }

        [HttpPost]
        [Route("~/upload/picture")]
        public ActionResult UploadPicture()
        {
            var xFileName = Request.Headers["X-File-Name"];
            return Json2(new { files = string.IsNullOrEmpty(xFileName) ? UploadWholeFile(Pictures) : UploadPartialFile(xFileName,Pictures) });
        }

        [HttpPost]
        [Route("~/upload/attachment")]
        public ActionResult UploadAttachment(int container)
        {
            var xFileName = Request.Headers["X-File-Name"];
            var files = string.IsNullOrEmpty(xFileName) ? UploadWholeFile(Documents) : UploadPartialFile(xFileName, Documents);
            var f = files.FirstOrDefault();
            if (f != null && f.Progress >= 100 && container>0)
            {
                Thread.Sleep(2000);//file may not be saved yet. wait for 2 seconds.
                var fileInfo=new FileInfo(Server.MapPath(string.Format("~/App_Data/Uploads/{0}",f.Name)));
                if (fileInfo.Exists)
                {
                    _service.CreateAttachment(new CreateAttachmentVM
                    {
                        Name = f.Original,
                        Size = fileInfo.Length,
                        ContentType = f.Type,
                        Url = f.Url
                    }, container);
                    f.Added = true;
                }
            }
            return Json2(new { files });
        }

        [HttpPost]
        [Route("~/upload/csv")]
        public ActionResult UploadContactList(int container)
        {
            var xFileName = Request.Headers["X-File-Name"];
            var files = string.IsNullOrEmpty(xFileName) ? UploadWholeFile(CSV) : UploadPartialFile(xFileName, CSV);
            var f = files.FirstOrDefault();
            if (f != null && f.Progress >= 100 && container>0)
            {
                var filePath = HostingEnvironment.MapPath(string.Format("~/App_Data/Uploads/{0}.csv", f.Key));
                _service.ImportSubscribers(System.IO.File.OpenText(filePath), container);
            }
            return Json2(new { files });
        }

        [Route("~/files/{filename}")]
        public ActionResult ViewPicture(string filename)
        {
            var f = new FileInfo(GetFullFileName(filename));
            return File(f.FullName, MediaTypeNames.Image.Jpeg);
        }

        private static ICollection<FilesStatus> MakeFileStatusError(string name, string error)
        {
            return new List<FilesStatus> { new FilesStatus(name) { Error = error } };
        }

        private ICollection<FilesStatus> UploadPartialFile(string fileName, IEnumerable<string> extensions)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return MakeFileStatusError(fileName, "No File Name Received");
            if (Request.Files == null || Request.Files.Count != 1)
                return MakeFileStatusError(fileName, "Attempt to upload chunked file containing more than one fragment per request");
            if (Request.Files[0] == null) return MakeFileStatusError(fileName, "No Files Received");

            var inputStream = Request.Files[0].InputStream;
            var status = new FilesStatus(new FileInfo(fileName));
            var ext = Path.GetExtension(fileName);
            if (!extensions.Contains(ext.ToLower()))
                return MakeFileStatusError(fileName, string.Format("{0} not allowed", ext));
            var fullName = GetFullFileName(status.Name);
            using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
            {
                var buffer = new byte[1024];

                var l = inputStream.Read(buffer, 0, 1024);
                while (l > 0)
                {
                    fs.Write(buffer, 0, l);
                    l = inputStream.Read(buffer, 0, 1024);
                }
                fs.Flush();
                fs.Close();
            }
            status.Size = (int)new FileInfo(fullName).Length;
            return new List<FilesStatus> { status };
        }

        private string GetFullFileName(string fileName)
        {
            return Path.Combine(_storageRoot, Path.GetFileName(fileName));
        }

        // Upload entire file
        private ICollection<FilesStatus> UploadWholeFile(string[] extensions)
        {
            var statuses = new List<FilesStatus>();
            for (var i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                if (file == null) continue;
                if (file.FileName == null) continue;
                var status = new FilesStatus(new FileInfo(file.FileName));
                var ext = Path.GetExtension(file.FileName);
                if (!extensions.Contains(ext.ToLower()))
                    return MakeFileStatusError(file.FileName, string.Format("{0} not allowed", ext));
                file.SaveAs(GetFullFileName(status.Name));
                statuses.Add(status);
            }
            return statuses;
        }
    }
}
