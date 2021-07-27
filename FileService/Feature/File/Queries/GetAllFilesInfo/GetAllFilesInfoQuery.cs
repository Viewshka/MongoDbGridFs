using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FileService.Options.MongoDb;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace FileService.Feature.File.Queries.GetAllFilesInfo
{
    public class GetAllFilesInfoQuery : IRequest<IList<AllFilesInfoDto>>
    {
    }

    public class GetAllFilesInfoQueryHandler : IRequestHandler<GetAllFilesInfoQuery, IList<AllFilesInfoDto>>
    {
        private const decimal OneKiloByte = 1024M;
        private const decimal OneMegaByte = OneKiloByte * 1024M;
        private const decimal OneGigaByte = OneMegaByte * 1024M;

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
                .Project(info => new AllFilesInfoDto
                {
                    Id = info.Id,
                    FileName = info.Filename,
                    Size = SizeToString(info.Length),
                    UploadDate = info.UploadDateTime,
                    Extension = GetExtension(info.Filename),
                    ContentType = info.Metadata.FirstOrDefault(m => m.Name == "ContentType").Value.AsString
                })
                .ToListAsync(cancellationToken);
        }

        private static string SizeToString(decimal size)
        {
            string suffix;
            switch (size)
            {
                case > OneGigaByte:
                    size /= OneGigaByte;
                    suffix = "Гб";
                    break;
                case > OneMegaByte:
                    size /= OneMegaByte;
                    suffix = "Мб";
                    break;
                case > OneKiloByte:
                    size /= OneKiloByte;
                    suffix = "Кб";
                    break;
                default:
                    suffix = "Б";
                    break;
            }

            return $"{size:#.##} {suffix}";
        }

        private static string GetExtension(string filename) => filename.Split('.').Last();
    }
}