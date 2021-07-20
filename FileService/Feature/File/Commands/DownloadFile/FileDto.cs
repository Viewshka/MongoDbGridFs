using System.IO;

namespace FileService.Feature.File.Commands.DownloadFile
{
    public class FileDto
    {
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public MemoryStream Content { get; set; }
    }
}