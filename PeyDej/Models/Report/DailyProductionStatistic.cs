using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Report;

[Table(name: "DailyProductionStatistics", Schema = "Report")]
public class DailyProductionStatistic
{
    [Key] public long Id { get; set; }

    public DateTime InsDate { get; set; } = DateTime.Now;

    [Display(Name = "شیفت")]
    [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
    public long ShiftId { get; set; }

    [Display(Name = "دپارتمان")]
    [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
    public long DepartmentId { get; set; }

    [Display(Name = "تعداد درگاه باز شده")]
    public long? ProductionCount { get; set; }

    [Display(Name = "ساعات توقف")]
    public long? StopsHour { get; set; }

    public long DailyStatisticsId { get; set; }

}