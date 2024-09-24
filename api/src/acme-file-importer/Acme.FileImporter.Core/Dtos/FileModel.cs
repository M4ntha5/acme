namespace Acme.FileImporter.Core.Dtos;

public class FileModel
{
    public string? Name { get; set; }

    public byte[]? Content { get; set; }

    public long? ContentLength { get; set; }

    public string? ContentType { get; set; }
}