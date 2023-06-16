using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Report;

[Table("RepairReport", Schema = "Report")]
public class RepairReport
{
    public RepairReport()
    {
        RepairRequest = default;
        AgendumOrder = default;
    }
    public long Id { get; set; }

    public DateTime InsDate { get; set; }

    public long RepairUnitAgendumOrderId { get; set; }

    [Display(Name = "تاریخ شروع کار")]
    [NotMapped]
    public string StartRepairDateDto { get; set; }
    public DateTime StartRepairDate { get; set; }

    [Display(Name = "ساعت شروع کار")]
    public long? StartRepairHour { get; set; }

    [Display(Name = "دقیقه شروع کار")]
    public long? StartRepairMinute { get; set; }


    [Display(Name = "شرح ایراد")]
    public string? FaultCaption { get; set; }

    [Display(Name = "توضیحات ")]
    public string? WorkDescription { get; set; }


    [Display(Name = "تاریخ پایان تعمیر")]
    [NotMapped]
    public string EndRepairDateDto { get; set; }
    public DateTime EndRepairDate { get; set; }

    [Display(Name = "ساعت پایان تعمیر")]
    public long? EndRepairHour { get; set; }

    [Display(Name = "دقیقه پایان تعمیر")]
    public long? EndRepairMinute { get; set; }

    [NotMapped]
    public RepairRequest? RepairRequest { get; set; }

    [NotMapped]
    public RepairUnitAgendumOrder? AgendumOrder { get; set; }
}
