using System;
using MongoDB.Bson;

namespace FileService.Feature.File.Queries.GetAllFilesInfo
{
    public class AllFilesInfoDto
    {
        public ObjectId Id { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public DateTime UploadDate { get; set; }
        public string Size { get; set; }
    }
}