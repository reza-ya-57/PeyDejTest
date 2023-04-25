using PeyDej.Models.Bases;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Report;

[Table("RepairReport", Schema = "Report")]
public partial class RepairReport
{
    public RepairReport()
    {
        People = new List<Person>();
    }
    public long Id { get; set; }

    public DateTime InsDate { get; set; }

    public long RepairRequestId { get; set; }

    [Display(Name = "عنوان")]
    public string? Caption { get; set; }

    [Display(Name = "تاریخ شروع")]
    public DateTime StartDate { get; set; }
    [NotMapped]
    [Display(Name = "تاریخ شروع")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public string StartDateDto { get; set; }

    [Display(Name = "تاریخ پایان")]
    public DateTime? EndDate { get; set; }
    [NotMapped]
    [Display(Name = "تاریخ پایان")]
    public string EndDateDto { get; set; }

    [Display(Name = "کاربر")]
    public long? PersonId { get; set; }
    [Display(Name = "بخش")]
    public long? PartId { get; set; }

    [NotMapped]
    public IEnumerable<Person> People { get; set; }
}
