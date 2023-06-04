using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace PeyDej.Models.Report;

[Table("MachineIR", Schema = "Report")]
public class MachineIR
{
    public long Id { get; set; }
    public DateTime InsDate { get; set; }
    public long MachineInspectionId { get; set; }
    public long CriteriaId { get; set; }
    public long? PersonId { get; set; }
    public int? Status { get; set; }
    public string? Description { get; set; }

}
