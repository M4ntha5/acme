using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Api.Models;

public class FormFileModel
{
    [Required]
    [FromForm]
    public required IFormFile File { get; init; }
}
