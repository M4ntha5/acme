using Acme.Core.SubscribersImporter.Dtos;

namespace Acme.Core.SubscribersImporter.Services.Interfaces;

public interface ISubscribersImporter
{
    Task<FileImportResponseDto> ImportSubscribers(ICollection<FileModel> files);
}
