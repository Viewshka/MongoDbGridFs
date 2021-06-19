using System;
using System.Linq;

namespace FileService.Models
{
    public class FileInfo
    {
        public FileInfo(string id, string fileName, long size, DateTime uploadDate, string contentType)
        {
            Id = id;
            FileName = fileName;
            Size = size;
            UploadDate = uploadDate;
            ContentType = contentType;
            Extension = fileName.Split('.').Last();
        }

        public string Id { get; }
        public string FileName { get; }
        public string Extension { get; }
        public long Size { get; }
        public DateTime UploadDate { get; }
        public string ContentType { get; }
    }
}