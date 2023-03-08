using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Bases;

[Table(name: "Machine", Schema = "Base")]
public class Machine
{
    public long Id { get; set; }

    public DateTime InsDate { get; set; }

    [Display(Name = "نام ماشین")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public string Name { get; set; } = null!;

    [Display(Name = "مدل ماشین")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public string Model { get; set; } = null!;

    /// <summary>
    /// دپارتمان
    /// </summary>
    [Display(Name = "دپارتمان")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public int Department { get; set; }

    /// <summary>
    /// شماره سریال
    /// </summary>
    public string SerialNumber { get; set; } = null!;

    /// <summary>
    /// کشور سازنده
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// شرکت سازنده
    /// </summary>
    public string? Company { get; set; }

    /// <summary>
    /// فرآیند
    /// </summary>
    public int Process { get; set; }

    /// <summary>
    /// وضعیت در زمان خرید
    /// </summary>
    public string? PurchaseStatus { get; set; }

    /// <summary>
    /// تاریخ بهره برداری
    /// </summary>
    public long? UtilizationDate { get; set; }

    /// <summary>
    /// آدرس کمپانی
    /// </summary>
    public string? CompanyAddress { get; set; }

    /// <summary>
    /// آدرس نمایندگی
    /// </summary>
    public string? AgencyAddress { get; set; }

    /// <summary>
    /// نوع انرژی
    /// </summary>
    public string? EnergyType { get; set; }

    /// <summary>
    /// میزان مصرف
    /// </summary>
    public long? EnergyConsumption { get; set; }

    /// <summary>
    /// نوع روغن
    /// </summary>
    public string? OilType { get; set; }

    /// <summary>
    /// میزان مصرف روعن
    /// </summary>
    public long? OilConsumption { get; set; }

    /// <summary>
    /// دروه تعویض روغن
    /// </summary>
    public long? OilReplacementPeriod { get; set; }

    /// <summary>
    /// نوع گریس
    /// </summary>
    public string? GreaseType { get; set; }

    /// <summary>
    /// محل روغن کاری
    /// </summary>
    public string? OilLocation { get; set; }

    /// <summary>
    /// میزان مصرف گریس
    /// </summary>
    public long? GreaseConsumption { get; set; }

    /// <summary>
    /// دوره تعویض گریس
    /// </summary>
    public long? GreaseReplacementPeriod { get; set; }

    /// <summary>
    /// محل گریسکاری
    /// </summary>
    public string? GreaseLocation { get; set; }

    /// <summary>
    /// تعداد گریس خور
    /// </summary>
    public int? GreaseCount { get; set; }

    public long? InspectionCycle { get; set; }

    public long? LubricationCycle { get; set; }

    public GeneralStatus GeneralStatusId { get; set; } = GeneralStatus.Active;
}