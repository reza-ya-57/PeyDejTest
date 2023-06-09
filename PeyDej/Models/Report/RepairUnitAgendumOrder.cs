using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using PeyDej.Models.Bases;

namespace PeyDej.Models.Report;

[Table("RepairUnitAgendumOrder", Schema = "Report")]
public class RepairUnitAgendumOrder
{
    public RepairUnitAgendumOrder()
    {
        ActionKinds = default;
        Locations = default;
    }
    public long Id { get; set; }

    public DateTime? InsDate { get; set; }

    public long RepairRequestId { get; set; }

    [Display(Name = "نوع اقدام")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public long ActionKindId { get; set; } //CategoryId = 14

    [NotMapped]
    public IQueryable<SubCategory>? ActionKinds { get; set; }

    [Display(Name = "محل تعمیر")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public long LocationId { get; set; } //CategoryId =  19

    [NotMapped]
    public IQueryable<SubCategory>? Locations { get; set; }

    [Display(Name = "نام کاربر")]
    [NotMapped]
    public List<long> PersonList { get; set; }

    public string? PersonIds { get; set; }

    [NotMapped]
    public IQueryable<Person>? Persons { get; set; }

    [Display(Name = "نوع اقدام تعمییرات")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    [NotMapped]
    public List<long> ActionTypes { get; set; }
    //[NotMapped]
    //public List<RepairReportSparePart> RepairReportSpareParts { get; set; }


    public virtual ICollection<RepairReportSparePart> RepairReportSparePartCollection { get; set; }

    [NotMapped]
    public IQueryable<SubCategory>? ActionTypeList { get; set; }
}
