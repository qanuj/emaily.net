using System.IO;

namespace Emaily.Web.Utils
{
    public class FilesStatus
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }
        public long Progress { get; set; }
        public string Url { get; set; }
        public string Error { get; set; }
        public string Original { get; set; }
        public string Key { get; set; }

        public FilesStatus() { }
        public FilesStatus(FileInfo fileInfo, bool isPicture = false) { SetValues(fileInfo.Name, fileInfo.Extension, isPicture); }

        public FilesStatus(FileInfo fileInfo, string fileError, bool isPicture = false)
        {
            SetValues(fileInfo.Name, fileInfo.Extension, isPicture, (int)fileInfo.Length);
            Error = fileError;
        }

        public FilesStatus(string fileName, string ext = "", bool isPicture = false, long fileLength = 0, string salt = "nothing")
        {
            SetValues(fileName, ext, isPicture, fileLength, salt);
        }
        private void SetValues(string fileName, string ext, bool isPicture = false, long fileLength = 0, string salt = "nothing")
        {
            Original = fileName;
            Key = Path.GetFileNameWithoutExtension(fileName) + "-" +
                  string.Format("{0}.{1}.{2}", fileName, fileLength, salt).CreateMD5();
            Name = Key + Path.GetExtension(fileName);
            Type = MimeTypeMap.GetMimeType(ext);
            Size = fileLength;
            Progress = 100;
            Url = "/" + (isPicture ? "picture" : "files") + "/" + Name;
        }
    }
}