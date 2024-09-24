using Acme.FileImporter.Core.Dtos;
using Acme.FileImporter.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Acme.FileImporter.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscribersImportController(ISubscribersImporter subscribersImporter) : Controller
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<FileImportResponseDto>> Import()
    {
        // TODO use Result pattern in a future
        try
        {
            var fileModels = new List<FileModel>();
            foreach (var file in HttpContext.Request.Form.Files)
            {
                await using var fileStream = file.OpenReadStream();
                var content = new byte[file.Length];
                _ = await fileStream.ReadAsync(content, 0, (int)file.Length);

                fileModels.Add(new FileModel
                {
                    Content = content,
                    ContentType = file.ContentType,
                    ContentLength = file.Length,
                    Name = file.Name,
                });
            }
        
            var result = await subscribersImporter.ImportSubscribers(fileModels);

            return Ok(new FileImportResponseDto(result.Errors, result.ExpiredSubscribers));
        }
        catch (Exception ex)
        {
            return Problem($"Error importing subscribers. {ex.Message}");
        }
    }
}
