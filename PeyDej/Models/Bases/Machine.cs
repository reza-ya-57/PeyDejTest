using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Bases;

[Table(name: "Machine", Schema = "Base")]
public class Machine
{
    [Key]
    public long Id { get; set; }

    public DateTime InsDate { get; set; } = DateTime.Now;

    [Display(Name = "نام ماشین")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public string Name { get; set; } = null!;

    [Display(Name = "مدل ماشین")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public string Model { get; set; } = null!;

    [Display(Name = "دپارتمان")]
    [Range(typeof(int), "0", "65536", ErrorMessage = "مقدار {0} باید بین {1} تا این {2} باشد")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public int? Department { get; set; }

    [Display(Name = "شماره سریال")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public string SerialNumber { get; set; } = null!;

    [Display(Name = "کشور سازنده")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? Country { get; set; }

    [Display(Name = "شرکت سازنده")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? Company { get; set; }

    [Display(Name = "فرآیند")]
    [Range(typeof(int), "0", "65536", ErrorMessage = "مقدار {0} باید بین {1} تا این {2} باشد")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public int? Process { get; set; }

    [Display(Name = "وضعیت در زمان خرید")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? PurchaseStatus { get; set; }

    [Display(Name = "تاریخ بهره برداری")]
    [Range(typeof(long), "0", "65536", ErrorMessage = "مقدار {0} باید بین {1} تا این {2} باشد")]
    public long? UtilizationDate { get; set; }

    [Display(Name = "آدرس کمپانی")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? CompanyAddress { get; set; }

    [Display(Name = "آدرس نمایندگی")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? AgencyAddress { get; set; }

    [Display(Name = "نوع انرژی")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? EnergyType { get; set; }

    [Display(Name = "میزان مصرف")]
    [Range(typeof(long), "0", "65536", ErrorMessage = "مقدار {0} باید بین {1} تا این {2} باشد")]
    public long? EnergyConsumption { get; set; }

    [Display(Name = "نوع روغن")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? OilType { get; set; }

    [Display(Name = "میزان مصرف روغن")]
    [Range(typeof(long), "0", "65536", ErrorMessage = "مقدار {0} باید بین {1} تا این {2} باشد")]
    public long? OilConsumption { get; set; }

    [Display(Name = "دوره تعویض روغن")]
    [Range(typeof(long), "0", "65536", ErrorMessage = "مقدار {0} باید بین {1} تا این {2} باشد")]
    public long? OilReplacementPeriod { get; set; }

    [Display(Name = "نوع گیریس")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? GreaseType { get; set; }

    [Display(Name = "محل روغن کاری")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? OilLocation { get; set; }

    [Display(Name = "میزان مصرف گیریس")]
    [Range(typeof(long), "0", "65536", ErrorMessage = "مقدار {0} باید بین {1} تا این {2} باشد")]
    public long? GreaseConsumption { get; set; }

    [Display(Name = "دوره تعویض گیریس")]
    [Range(typeof(long), "0", "65536", ErrorMessage = "مقدار {0} باید بین {1} تا این {2} باشد")]
    public long? GreaseReplacementPeriod { get; set; }

    [Display(Name = "محل گیریس کاری")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? GreaseLocation { get; set; }

    [Display(Name = "تعداد گیریس خور")]
    [Range(typeof(int), "0", "65536", ErrorMessage = "مقدار {0} باید بین {1} تا این {2} باشد")]
    public int? GreaseCount { get; set; }

    [Display(Name = "دوره بازرسی")]
    [Range(typeof(long), "0", "65536", ErrorMessage = "مقدار {0} باید بین {1} تا این {2} باشد")]
    public long? InspectionCycle { get; set; }

    [Display(Name = "چرخه روغن کاری")]
    [Range(typeof(long), "0", "65536", ErrorMessage = "مقدار {0} باید بین {1} تا این {2} باشد")]
    public long? LubricationCycle { get; set; }

    public GeneralStatus GeneralStatusId { get; set; } = GeneralStatus.Active;
}