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

        [Display(Name = "تعداد درگاه باز شده")]
        [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
        public long? OpenPortCount { get; set; }

        [Display(Name = "تعداد بارگیری")]
        [Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
        public long? LoadingCount { get; set; }
    }
}
