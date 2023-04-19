using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PeyDej.Models.Report;

public partial class RepairRequest
{
    public long Id { get; set; }

    public DateTime InsDate { get; set; }

    [Display(Name = "عنوان")]
    public string? Caption { get; set; }

    public long DepartmentId { get; set; }

    [Display(Name = "فرآیند")]
    public long? ProcessId { get; set; }

    [Display(Name = "تاریخ")]
    public DateTime? Date { get; set; }

    [Display(Name = "گزارش دهنده")]
    public string? Reporter { get; set; }

    [Display(Name = "نوع درخواست")]
    public long? RepairKindId { get; set; }

    [Display(Name = "ماشین")]
    public long? MachineId { get; set; }

    [Display(Name = "عنوان")]
    public long? MotorId { get; set; }

    [Display(Name = "عنوان")]
    public string? SparePartId { get; set; }

    [Display(Name = "عنوان")]
    public int? Status { get; set; }
}
