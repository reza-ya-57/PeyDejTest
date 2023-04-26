using System.ComponentModel.DataAnnotations;

namespace PeyDej.Models.Dtos
{
    public class LoadingReportDto
    {
        public LoadingReportDto()
        {
            LoadingReport_104_Id = 0;
            LoadingReport_105_Id = 0;
            BladKindId_101_LoadingIntervalId_104_Id = 0;
            BladKindId_102_LoadingIntervalId_104_Id = 0;
            BladKindId_103_LoadingIntervalId_104_Id = 0;
            BladKindId_101_LoadingIntervalId_105_Id = 0;
            BladKindId_102_LoadingIntervalId_105_Id = 0;
            BladKindId_103_LoadingIntervalId_105_Id = 0;
        }
        public long LoadingReport_104_Id { get; set; }
        public long LoadingReport_105_Id { get; set; }
        [Display(Name = "تاریخ")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
        [MaxLength(10, ErrorMessage = "مقدار {0} باید 10 کاراکتر باشد")]
        public string Date { get; set; }

        [Display(Name = "روز")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
        public string DayCaption { get; set; } = null!;

        [Display(Name = "نوع تیغه")]
        public long? BladKindId { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description_LoadingIntervalId_104 { get; set; }
        public long? BladKindId_101_LoadingIntervalId_104 { get; set; }
        public long? BladKindId_102_LoadingIntervalId_104 { get; set; }
        public long? BladKindId_103_LoadingIntervalId_104 { get; set; }

        public long BladKindId_101_LoadingIntervalId_104_Id { get; set; }
        public long BladKindId_102_LoadingIntervalId_104_Id { get; set; }
        public long BladKindId_103_LoadingIntervalId_104_Id { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description_LoadingIntervalId_105 { get; set; }
        public long? BladKindId_101_LoadingIntervalId_105 { get; set; }
        public long? BladKindId_103_LoadingIntervalId_105 { get; set; }
        public long? BladKindId_102_LoadingIntervalId_105 { get; set; }
        public long BladKindId_101_LoadingIntervalId_105_Id { get; set; }
        public long BladKindId_102_LoadingIntervalId_105_Id { get; set; }
        public long BladKindId_103_LoadingIntervalId_105_Id { get; set; }


    }
}
