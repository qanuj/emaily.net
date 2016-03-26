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
        public bool Added { get; set; }

        public FilesStatus() { }
        public FilesStatus(FileInfo fileInfo) { SetValues(fileInfo.Name, fileInfo.Extension); }

        public FilesStatus(FileInfo fileInfo, string fileError)
        {
            SetValues(fileInfo.Name, fileInfo.Extension, (int)fileInfo.Length);
            Error = fileError;
        }

        public FilesStatus(string fileName, string ext = "", long fileLength = 0, string salt = "nothing")
        {
            SetValues(fileName, ext, fileLength, salt);
        }
        private void SetValues(string fileName, string ext,  long fileLength = 0, string salt = "nothing")
        {
            Original = fileName;
            Key = Path.GetFileNameWithoutExtension(fileName) + "-" +
                  string.Format("{0}.{1}.{2}", fileName, fileLength, salt).CreateMD5();
            Name = Key + Path.GetExtension(fileName);
            Type = MimeTypeMap.GetMimeType(ext);
            Size = fileLength;
            Progress = 100;
            Url = "/files/" + Name;
        }
    }
}