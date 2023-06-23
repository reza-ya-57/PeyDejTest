using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PeyDej.Models.Report
{

    [Table(name: "DailyStatisticsProduction", Schema = "Report")]
    public class DailyStatisticsProduction
    {
        [Key] 
        public long Id { get; set; }

        [Display(Name = "تاریخ")]
        [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
        public string Date { get; set; }
        
        [Display(Name = "تعداد قفسه خیس")]
        [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
        public long? WetShelfCount { get; set; }

        [Display(Name = "تعداد قفسه خشک")]
        [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
        public long? DryShelfCount { get; set; }
        [Display(Name = "ساعت توفقات خیس")]
        public int? WetStopHour { get; set; }
        [Display(Name = "دقیقه توقفات خیس")]
        public int? WetStopMinute { get; set; }
        [Display(Name = "ساعت توفقات خشک")]
        public int? DryStopHour { get; set; }
        [Display(Name = "دقیقه توقفات خشک")]
        public int? DryStopMinute { get; set; }
    }
}
