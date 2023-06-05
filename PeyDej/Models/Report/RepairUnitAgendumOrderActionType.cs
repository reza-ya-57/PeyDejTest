using PeyDej.Models.Bases;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace PeyDej.Models.Report;

[Table("RepairUnitAgendumOrderActionType", Schema = "Report")]
public class RepairUnitAgendumOrderActionType
{
    public RepairUnitAgendumOrderActionType()
    {
        ActionTypes = default;
    }
    public long Id { get; set; }
    public DateTime? InsDate { get; set; }
    public long RepairUnitAgendumOrderId { get; set; }
    [Display(Name = "نوع اقدام تعمییرات")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public long ActionTypeId { get; set; }

    [NotMapped]
    public IQueryable<SubCategory>? ActionTypes { get; set; }
}
