using System;
using System.Linq;

namespace FileService.Feature.File.Queries.GetFileInfo
{
    public class FileInfoDto
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }
        public DateTime UploadDate { get; set; }
        public string ContentType { get; set; }

        public string Extension => FileName.Split('.').Last();
    }
}