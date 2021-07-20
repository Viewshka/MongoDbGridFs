using System.Threading;
using System.Threading.Tasks;
using FileService.Options.MongoDb;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace FileService.Feature.File.Commands.UploadFile
{
    public class UploadFileCommand : IRequest<string>
    {
        public IFormFile File { get; set; }
    }

    public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, string>
    {
        private readonly MongoDbOptions _options;

        public UploadFileCommandHandler(IOptions<MongoDbOptions> options)
        {
            _options = options.Value;
        }

        public async Task<string> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            var client = new MongoClient(_options.ConnectionString);
            var database = client.GetDatabase(_options.DatabaseName);
            var gridFs = new GridFSBucket(database);

            var fileId = await gridFs.UploadFromStreamAsync(request.File.FileName,
                request.File.OpenReadStream(),
                new GridFSUploadOptions
                {
                    Metadata = new BsonDocument
                    {
                        new BsonElement("ContentType", request.File.ContentType)
                    }
                }, cancellationToken);

            return fileId.ToString();
        }
    }
}