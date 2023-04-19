using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PeyDej.Models.Report;

public partial class RepairReport
{
    public long Id { get; set; }

    public DateTime InsDate { get; set; }

    public long RepairRequestId { get; set; }

    [Display(Name = "عنوان")]
    public string? Caption { get; set; }

    [Display(Name = "تاریخ شروع")]
    public DateTime StartDate { get; set; }

    [Display(Name = "تاریخ پایان")]
    public DateTime? EndDate { get; set; }

    [Display(Name = "کاربر")]
    public long? PersonId { get; set; }
}
