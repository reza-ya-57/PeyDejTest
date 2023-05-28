using PeyDej.Enums;

namespace PeyDej.Service.File;

public interface IFileInterface
{
    public Guid AddFile(UploadFor catalog, IFormFile file);
    public Guid EditFile(UploadFor catalog, IFormFile file, Guid idOldFile);
}