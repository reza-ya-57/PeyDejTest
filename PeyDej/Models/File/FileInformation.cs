using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.File;

[Table(name: "FileInformation", Schema = "dbo")]
public class FileInformation
{
    public FileInformation()
    {
    }
    public FileInformation(IFormFile file, string fileLocation) : this()
    {
        Id = Guid.NewGuid();
        FileLocation = fileLocation;
        FileName = file.FileName.Split('.')[0];
        FileExtension = Path.GetExtension(file.FileName);
        Size = file.Length;
        ContentType = file.ContentType;
    }
    public Guid Id { get; set; }

    [StringLength(150)]
    public string? FileLocation { get; set; }

    [StringLength(20)]
    public string? FileExtension { get; set; }

    [StringLength(50)]
    public string? ContentType { get; set; }

    public long? Size { get; set; }

    [StringLength(128)]
    public string? FileName { get; set; }
}
