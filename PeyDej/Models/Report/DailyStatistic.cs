//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PeyDej.Models.Report;

//[Table(name: "DailyStatistics", Schema = "Report")]
//public class DailyStatistic
//{
//    [Key] 
//    public long Id { get; set; }

//    public DateTime InsDate { get; set; } = DateTime.Now;

//    [Display(Name = "تاریخ")]
//    [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
//    [StringLength(10,ErrorMessage = "طول {0} باید {1} کارکتر باشد")]
//    public string Date { get; set; }
    
//    [Display(Name = "روز")]
//    [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
//    public int Week { get; set; }

//    /// <summary>
//    /// تعداد درگاه بازشده
//    /// </summary>
//    [Display(Name = "تعداد درگاه باز شده")]
//    [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
//    public long? NumberOfOpenPort { get; set; }

//    /// <summary>
//    /// تعداد بارگیری شده
//    /// </summary>
//    [Display(Name = "تعداد بارگیری شده")]
//    [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
//    public long? LoadingCount { get; set; }

//}