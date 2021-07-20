using System.Threading;
using System.Threading.Tasks;
using FileService.Options.MongoDb;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace FileService.Feature.File.Commands.DeleteFile
{
    public class DeleteFileCommand : IRequest
    {
        public string Id { get; set; }
    }

    public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand>
    {
        private readonly MongoDbOptions _options;

        public DeleteFileCommandHandler(IOptions<MongoDbOptions> options)
        {
            _options = options.Value;
        }

        public async Task<Unit> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            var client = new MongoClient(_options.ConnectionString);
            var database = client.GetDatabase(_options.DatabaseName);
            var gridFs = new GridFSBucket(database);

            var objectId = ObjectId.Parse(request.Id);
            await gridFs.DeleteAsync(objectId, cancellationToken);
            
            return Unit.Value;
        }
    }
}