using Acme.Core.SubscribersImporter.Dtos;
using Acme.Core.SubscribersImporter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Api.Controllers;

[Route("api/[controller]")]
public class SubscribersImportController(ISubscribersImporter subscribersImporter) : Controller
{
    [HttpPost]
    public async Task<FileImportResponseDto> Import()
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
        
        return new FileImportResponseDto(result.Errors, result.ExpiredSubscribers);
    }
}
