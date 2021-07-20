using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FileService.Options.MongoDb;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace FileService.Feature.File.Commands.DownloadFile
{
    public class DownloadFileCommand : IRequest<FileDto>
    {
        public string Id { get; set; }
    }

    public class DownloadFileCommandHandler : IRequestHandler<DownloadFileCommand, FileDto>
    {
        private readonly MongoDbOptions _options;

        public DownloadFileCommandHandler(IOptions<MongoDbOptions> options)
        {
            _options = options.Value;
        }

        public async Task<FileDto> Handle(DownloadFileCommand request, CancellationToken cancellationToken)
        {
            var client = new MongoClient(_options.ConnectionString);
            var database = client.GetDatabase(_options.DatabaseName);
            var gridFs = new GridFSBucket(database);

            var objectId = ObjectId.Parse(request.Id);
            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", objectId);
            var files = await gridFs.FindAsync(filter, cancellationToken: cancellationToken);
            var file = await files.FirstOrDefaultAsync(cancellationToken);

            if (file is null) throw new Exception("Файл не найден");

            var memoryStream = new MemoryStream();
            await gridFs.DownloadToStreamAsync(objectId, memoryStream, cancellationToken: cancellationToken);
            memoryStream.Position = 0;

            return new FileDto
            {
                ContentType = file.Metadata["ContentType"].AsString,
                FileName = file.Filename,
                Content = memoryStream
            };
        }
    }
}