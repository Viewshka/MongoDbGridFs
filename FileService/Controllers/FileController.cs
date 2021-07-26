using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using FileService.Common;
using FileService.Feature.File.Commands.DeleteFile;
using FileService.Feature.File.Commands.DownloadFile;
using FileService.Feature.File.Commands.UploadFile;
using FileService.Feature.File.Queries.GetAllFilesInfo;
using FileService.Feature.File.Queries.GetFileInfo;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FileController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        ///     Получить информацию о файле
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/info")]
        public async Task<IActionResult> GetFileInfo(string id)
        {
            return Ok(await _mediator.Send(new GetFileInfoQuery {Id = id}));
        }

        /// <summary>
        /// Получить информацию о всех файлах
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAsync(DataSourceLoadOptions loadOptions)
        {
            loadOptions.RequireTotalCount = true;
            return Ok(DataSourceLoader.Load(await _mediator.Send(new GetAllFilesInfoQuery()), loadOptions));
        }

        /// <summary>
        ///     Скачать файл
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadFile(string id)
        {
            var file = await _mediator.Send(new DownloadFileCommand {Id = id});

            return File(file.Content, file.ContentType, file.FileName);
        }

        /// <summary>
        ///     Загрузить файл
        /// </summary>
        /// <returns></returns>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile()
        {
            var file = Request.Form.Files[0];

            return Ok(await _mediator.Send(new UploadFileCommand {File = file}));
        }

        /// <summary>
        ///     Удаление файла по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(string id)
        {
            await _mediator.Send(new DeleteFileCommand {Id = id});
            return Ok();
        }
    }
}