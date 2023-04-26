using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Report;

[Table("RepairRequest",Schema = "Report")]
public partial class RepairRequest
{
    public long Id { get; set; }

    public DateTime InsDate { get; set; }

    [Display(Name = "عنوان")]

    [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
    public string? Caption { get; set; }

    [DisplayName("دپارتمان")]
    public long DepartmentId { get; set; }

    [Display(Name = "فرآیند")]
    public long? ProcessId { get; set; }

    [Display(Name = "تاریخ")]
    [NotMapped]
    public string? DateDto { get; set; }

    [Display(Name = "تاریخ")]
    public DateTime? Date { get; set; }

    [Display(Name = "گزارش دهنده")]
    public string? Reporter { get; set; }

    [Display(Name = "نوع درخواست")]
    public long? RepairKindId { get; set; }

    [Display(Name = "ماشین")]
    public long? MachineId { get; set; }

    [Display(Name = "موتور")]
    public long? MotorId { get; set; }

    [Display(Name = "قطعه")]
    public long? SparePartId { get; set; }

    [Display(Name = "عنوان")]
    public int? Status { get; set; }
    [Display(Name = "بخش")]
    public long? PartId { get; set; }

    [Display(Name = "درخواست از")]
    public long? PersonFromId { get; set; }

    [Display(Name = "درخواست به")]
    public long? PersonToId { get; set; }

    [Display(Name = "ساعت")]
    public int? Hour { get; set; }
}
