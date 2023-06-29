using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PeyDej.Models.Bases;

namespace PeyDej.Models.Report;

[Table("DailyWetProductIR", Schema = "Report")]
public class DailyWetProductIR
{
    public DailyWetProductIR()
    {
        DaneBandiGarkList = default;
        TemplateKindList = default;
        SoilTypeList = default;
        InsDate = DateTime.Now;
        GeneralStatusId = GeneralStatus.Active;
    }
    public long Id { get; set; }
    public GeneralStatus GeneralStatusId { get; set; }
    public DateTime InsDate { get; set; }
    public DateTime Date { get; set; }
    [NotMapped]
    [Display(Name = "تاریخ")]
    public string DateDto { get; set; }

    //[Display(Name = "دما و رطوبت")]
    //public int? TemperatureAndHumidityStatus { get; set; }

    [Display(Name = "ابعاد تیغه")]
    public long? BladeDimensionX { get; set; }

    //[Display(Name = "ابعاد تیغه")]
    //public long? BladeDimensionY { get; set; }

    [Display(Name = "شماره قالب")]
    public long? TemplateNumber { get; set; }

    [Display(Name = "وزن تیغه")]
    public float? BladeWeight { get; set; }

    [Display(Name = "نام پرستار شیفت روز")]
    public long? MorningShiftOperatorPersonId { get; set; }
    [Display(Name = "نام پرستار شیفت شب")]
    public long? NightShiftOperatorPersonId { get; set; }
    [NotMapped]
    [Display(Name = "نام پرستار شیفت روز")]
    public IQueryable<Person>? ShiftOperatorPerson { get; set; }



    [Display(Name = "نوع گل")]
    public long? SoilTypeId { get; set; }//نوع گل Category = 17

    [NotMapped]
    [Display(Name = "نوع گل")]
    public IQueryable<SubCategory>? SoilTypeList { get; set; }

    [Display(Name = "دور گل")]
    public long? SoilSpeed { get; set; }

    [Display(Name = "درجه وکیوم")]
    public long? VaciumDegree { get; set; }

    [Display(Name = "نوع بند")]
    public int? BandStatus { get; set; }

    [Display(Name = "نسبت ترکیب خاک")]
    public float? SoilCompoundRatio { get; set; }

    [Display(Name = "دانه بندی گرک ")]
    public long? DaneBandiGarkId { get; set; }//دانه بندی گرک CategoryId = 22

    [NotMapped]
    [Display(Name = "دانه بندی گرک ")]
    public IQueryable<SubCategory>? DaneBandiGarkList { get; set; }

    [Display(Name = "نوع قالب ")]
    public long? TemplateKindId { get; set; }//نوع قالب CategoryId = 21

    [NotMapped]
    [Display(Name = "نوع قالب ")]
    public IQueryable<SubCategory>? TemplateKindList { get; set; }

    [Display(Name = "شماره شروع فقسات ")]
    public long? ShelfNumberStart { get; set; }

    [Display(Name = "شماره پایان فقسات ")]
    public long? ShelfNumberEnd { get; set; }
}

