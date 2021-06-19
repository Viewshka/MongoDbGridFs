using System;
using System.IO;
using System.Threading.Tasks;
using FileService.Options.MongoDb;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace FileService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        private readonly IGridFSBucket _gridFs;

        public FileController(ILogger<FileController> logger, IOptions<MongoDbOptions> options)
        {
            _logger = logger;

            var client = new MongoClient(options.Value.ConnectionString);
            var database = client.GetDatabase(options.Value.DatabaseName);
            _gridFs = new GridFSBucket(database);
        }

        /// <summary>
        /// Получить информацию о файле
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}/info")]
        public async Task<IActionResult> GetFileInfo(string id)
        {
            var objectId = ObjectId.Parse(id);
            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", objectId);
            var files = await _gridFs.FindAsync(filter);
            var file = await files.FirstOrDefaultAsync();

            if (file is null) throw new Exception("Файл не найден");

            var result = new Models.FileInfo(id,
                file.Filename,
                file.Length,
                file.UploadDateTime,
                file.Metadata["ContentType"].AsString);
            
            var message = $"Запрос информации о файле: id={id}/filename={result.FileName}";
            _logger.LogInformation(message);

            return Ok(result);
        }

        /// <summary>
        /// Скачать файл
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadFile(string id)
        {
            var objectId = ObjectId.Parse(id);
            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", objectId);
            var files = await _gridFs.FindAsync(filter);
            var file = await files.FirstOrDefaultAsync();
            
            if (file is null) throw new Exception("Файл не найден");
            
            var memoryStream = new MemoryStream();
            await _gridFs.DownloadToStreamAsync(objectId, memoryStream);
            memoryStream.Position = 0;
            
            var message = $"Скачан файл: id={id}/filename={file.Filename}";
            _logger.LogInformation(message);
            
            return File(memoryStream, file.Metadata["ContentType"].AsString, file.Filename);
        }

        /// <summary>
        /// Загрузить файл
        /// </summary>
        /// <returns></returns>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile()
        {
            var file = Request.Form.Files[0];
            var fileId = await _gridFs.UploadFromStreamAsync(file.FileName,
                file.OpenReadStream(),
                new GridFSUploadOptions
                {
                    Metadata = new BsonDocument
                    {
                        new BsonElement("ContentType", file.ContentType)
                    }
                });

            var message = $"Загружен новый файл: id={fileId}/filename={file.FileName}";
            _logger.LogInformation(message);
            
            return Ok(fileId.ToString());
        }
    }
}