using PeyDej.Models.Bases;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Report;

[Table("DailyDryProductIR", Schema = "Report")]
public class DailyDryProductIR
{
    public DailyDryProductIR()
    {
        ShiftOperatorPerson = default;
        TemplateKindList = default;
        GeneralStatusId = GeneralStatus.Active;
    }
    public long Id { get; set; }
    public GeneralStatus GeneralStatusId { get; set; }
    public DateTime Date { get; set; }
    [NotMapped]
    [Display(Name = "تاریخ")]
    public string DateDto { get; set; }
    [Display(Name = "دما و رطوبت")]
    public float? TemperatureAndHumidity { get; set; }
    [Display(Name = "دمای خشک کن")]
    public float? DryerTemperature { get; set; }
    [Display(Name = "ابعاد تیغه")]
    public long? BladeDimensionX { get; set; }
    [Display(Name = "ابعاد تیغه")]
    public long? BladeDimensionY { get; set; }


    [Display(Name = "نام پرستار شیفت روز")]
    public long? MorningShiftOperatorPersonId { get; set; }
    [Display(Name = "نام پرستار شیفت شب")]
    public long? NightShiftOperatorPersonId { get; set; }
    [NotMapped]
    [Display(Name = "نام پرستار شیفت روز")]
    public IQueryable<Person>? ShiftOperatorPerson { get; set; }


    [Display(Name = "لب پریدگی")]
    public int? LipTwitchingStatus { get; set; }
    [Display(Name = "ترک داخلی")]
    public int? InternalCrackStatus { get; set; }
    [Display(Name = "ترک خارجی")]
    public int? TemplateExternalCrackStatus { get; set; }
    [Display(Name = "وضعیت ترک خارجی")]
    public int? DryerExternalCrackStatus { get; set; }

    [Display(Name = "نوع قالب")]
    public long? TemplateKindId { get; set; }

    [NotMapped]
    [Display(Name = "نوع قالب ")]
    public IQueryable<SubCategory>? TemplateKindList { get; set; }


    [Display(Name = "شماره شروع فقسات ")]
    public long? ShelfNumberStart { get; set; }
    [Display(Name = "شماره پایان فقسات ")]
    public long? ShelfNumberEnd { get; set; }
    [Display(Name = "تعداد پالت های نم دار ")]
    public long? WetPalletCount { get; set; }
    [Display(Name = "محدوده نمدار فقسه ")]
    public long? WetShelfRange { get; set; }
    [Display(Name = "شماره کانال های نم دار ")]
    public string? WetChannelNumber { get; set; }
}

