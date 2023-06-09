using PeyDej.Models.Bases;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace PeyDej.Models.Report;

[Table("RepairUnitAgendumOrderActionType", Schema = "Report")]
public class RepairUnitAgendumOrderActionType
{
    public long Id { get; set; }
    public DateTime? InsDate { get; set; }
    public long RepairUnitAgendumOrderId { get; set; }
    public long ActionTypeId { get; set; }
}
