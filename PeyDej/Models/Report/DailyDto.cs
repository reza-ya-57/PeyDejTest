using System.ComponentModel.DataAnnotations;

namespace PeyDej.Models.Report;

public class DailyDto
{
    [Key] public long Id { get; set; }

    public DateTime InsDate { get; set; } = DateTime.Now;

    [Display(Name = "تاریخ")]
    [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
    [StringLength(10, ErrorMessage = "طول {0} باید {1} کارکتر باشد")]
    public string Date { get; set; }

    [Display(Name = "روز")]
    [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
    public int Week { get; set; }

    /// <summary>
    /// تعداد درگاه بازشده
    /// </summary>
    [Display(Name = "تعداد درگاه بازشده")]
    [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
    public long? NumberOfOpenPort { get; set; }

    /// <summary>
    /// تعداد بارگیری شده
    /// </summary>
    [Display(Name = "تعداد بارگیری شده")]
    [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
    public long? LoadingCount { get; set; }


    //___________________________________________


    [Display(Name = "شیفت1")]
    [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
    public long ShiftId_99_DepartmentId_106_StopsHour { get; set; }

    [Display(Name = "2شیفت")]
    [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
    public long ShiftId_99_DepartmentId_107_StopsHour { get; set; }
    

    [Display(Name = "شیفت1")]
    [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
    public long ShiftId_100_DepartmentId_106_StopsHour { get; set; }

    [Display(Name = "2شیفت")]
    [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
    public long ShiftId_100_DepartmentId_107_StopsHour { get; set; }



    [Display(Name = "شیفت1")]
    [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
    public long ShiftId_99_DepartmentId_106_ProductionCount { get; set; }

    [Display(Name = "2شیفت")]
    [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
    public long ShiftId_99_DepartmentId_107_ProductionCount { get; set; }


    [Display(Name = "شیفت1")]
    [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
    public long ShiftId_100_DepartmentId_106_ProductionCount { get; set; }

    [Display(Name = "2شیفت")]
    [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
    public long ShiftId_100_DepartmentId_107_ProductionCount { get; set; }


    public long DailyStatisticsId { get; set; }

}