using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PeyDej.Models.Report;

[Table("RepairRequest",Schema = "Report")]
public class RepairRequest
{
    public long Id { get; set; }

    public DateTime? InsDate { get; set; }

    [Display(Name = "عنوان")]

    [StringLength(450)]
    public string? UserId { get; set; }
    [Display(Name = "ماشین")]
    public long? MachineId { get; set; }

    [DisplayName("شرح ")]
    public string Caption { get; set; }

    public GeneralStatus GeneralStatusId { get; set; }

    [Display(Name = " رسیدگی شده یا نشده")]
    public int Status { get; set; }
}
