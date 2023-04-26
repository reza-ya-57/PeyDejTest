using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PeyDej.Models.Bases;

namespace PeyDej.Models.Inspection;

[Table(name: "MachineLubricationIS", Schema = "Inspection")]
public class MachineLubricationIS
{
    [Key] 
    public long Id { get; set; }

    public DateTime InsDate { get; set; } = DateTime.Now;

    [Display(Name = "نام ماشین")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public long MachineId { get; set; }

    [Display(Name = "تاریخ بازرسی")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public DateTime InspectionDate { get; set; }

    [Display(Name = "تاریخ پایان بازرسی")]
    public DateTime? InspectionFinishedDate { get; set; }

    [Display(Name = "وضعیت بازرسی")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public int Status { get; set; } = 0;

    [NotMapped]
    public virtual IEnumerable<Machine> Machines { get; } = new List<Machine>();
}