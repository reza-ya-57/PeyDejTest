using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Report;

[Table("MotorIR", Schema = "Report")]
public class MotorIR
{
    public long Id { get; set; }
    public DateTime InsDate { get; set; }
    public long MotorInspectionId { get; set; }
    public long PersonId { get; set; }
    public int Status { get; set; }
    public string? Description { get; set; }
}
