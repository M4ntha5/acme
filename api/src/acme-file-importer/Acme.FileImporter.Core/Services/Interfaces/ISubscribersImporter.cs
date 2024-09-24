using Acme.FileImporter.Core.Dtos;

namespace Acme.FileImporter.Core.Services.Interfaces;

public interface ISubscribersImporter
{
    Task<FileImportResponseDto> ImportSubscribers(ICollection<FileModel> files);
}
