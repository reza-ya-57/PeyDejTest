using System.ComponentModel.DataAnnotations;

namespace PeyDej.Models.Dtos;
public class InspectionDto
{
    public long MachineId { get; set; }

    [Display(Name = "نام ماشین")]
    public string Name { get; set; }

    [Display(Name = "مدل ماشین")]
    public string Model { get; set; }
    public int? Status { get; set; }
    public string? Description { get; set; }
    public long? PersonId{ get; set; }
}

