using System;
using System.Threading;
using System.Threading.Tasks;
using FileService.Options.MongoDb;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace FileService.Feature.File.Queries.GetFileInfo
{
    public class GetFileInfoQuery : IRequest<FileInfoDto>
    {
        public string Id { get; set; }
    }

    public class GetFileInfoQueryHandler : IRequestHandler<GetFileInfoQuery, FileInfoDto>
    {
        private readonly MongoDbOptions _options;

        public GetFileInfoQueryHandler(IOptions<MongoDbOptions> options)
        {
            _options = options.Value;
        }

        public async Task<FileInfoDto> Handle(GetFileInfoQuery request, CancellationToken cancellationToken)
        {
            var client = new MongoClient(_options.ConnectionString);
            var database = client.GetDatabase(_options.DatabaseName);
            var gridFs = new GridFSBucket(database);

            var objectId = ObjectId.Parse(request.Id);
            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", objectId);
            var files = await gridFs.FindAsync(filter, cancellationToken: cancellationToken);
            var file = await files.FirstOrDefaultAsync(cancellationToken);

            if (file is null) throw new Exception("Файл не найден");

            return new FileInfoDto
            {
                Id = request.Id,
                FileName = file.Filename,
                Size = file.Length,
                UploadDate = file.UploadDateTime,
                ContentType = file.Metadata["ContentType"].AsString
            };
        }
    }
}