using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FileService.Options.MongoDb;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace FileService.Feature.File.Queries.GetAllFilesInfo
{
    public class GetAllFilesInfoQuery : IRequest<IList<AllFilesInfoDto>>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class GetAllFilesInfoQueryHandler : IRequestHandler<GetAllFilesInfoQuery, IList<AllFilesInfoDto>>
    {
        private readonly MongoDbOptions _options;

        public GetAllFilesInfoQueryHandler(IOptions<MongoDbOptions> options)
        {
            _options = options.Value;
        }

        public async Task<IList<AllFilesInfoDto>> Handle(GetAllFilesInfoQuery request,
            CancellationToken cancellationToken)
        {
            var client = new MongoClient(_options.ConnectionString);
            var database = client.GetDatabase(_options.DatabaseName);
            
            var filter = Builders<GridFSFileInfo>.Filter.Empty;
            var files = database.GetCollection<GridFSFileInfo>("fs.files");

            return await files
                .Find(filter)
                .Skip(request.Skip)
                .Limit(request.Take)
                .Project(info => new AllFilesInfoDto
                {
                    Id = info.Id,
                    FileName = info.Filename,
                    Size = SizeToString(info.Length),
                    UploadDate = info.UploadDateTime,
                    Extension = GetExtension(info.Filename)
                })
                .ToListAsync(cancellationToken);
        }

        private static string SizeToString(long length) => $"{length / 1024} кб";
        
        private static string GetExtension(string filename) => filename.Split('.').Last();
    }
}