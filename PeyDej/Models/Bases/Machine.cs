using PeyDej.Models.Dtos;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PeyDej.Models.Bases;

[Table(name: "Machine", Schema = "Base")]
public class Machine
{
    public Machine()
    {
        Motors = new List<Motor>();
        SpareParts = new List<SparePart>();
        SparePartMachines = new List<SparePartMachine>();
        DepartmentIds = new List<CategoryDto>();
        ProcessIds = new List<CategoryDto>();
    }
    [Key] public long Id { get; set; }

    public DateTime InsDate { get; set; } = DateTime.Now;
    [NotMapped]
    public long MachineISId { get; set; }
    [Display(Name = "نام ماشین")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public string Name { get; set; } = null!;

    [Display(Name = "مدل ماشین")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public string Model { get; set; } = null!;


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


    [Display(Name = "وضعیت در زمان خرید")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? PurchaseStatus { get; set; }

    [Display(Name = "تاریخ بهره برداری")]
    public DateTime? UtilizationDate { get; set; }
    [Display(Name = "تاریخ بهره برداری")]
    [NotMapped]
    public string? UtilizationDateDto { get; set; }

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
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public long? InspectionCycle { get; set; }

    [Display(Name = "چرخه روغن کاری")]
    [Range(typeof(long), "0", "65536", ErrorMessage = "مقدار {0} باید بین {1} تا این {2} باشد")]
    public long? LubricationCycle { get; set; }



    [Display(Name = "فرآیند")]
    public int? Process { get; set; }

    [NotMapped]
    public IEnumerable<CategoryDto> ProcessIds { get; set; }



    [DisplayName("دپارتمان")]
    public int? Department { get; set; }

    [NotMapped]
    public IEnumerable<CategoryDto> DepartmentIds { get; set; }

    [DisplayName("موتور")]
    [NotMapped]
    public List<long>? MotorIds { get; set; }

    [NotMapped]
    public IEnumerable<Motor> Motors { get; set; }

    [DisplayName("قطعات")]
    [NotMapped]
    public List<long>? SparePartIds { get; set; }

    [Display(Name = "تاریخ بازرسی")]
    public DateTime? InspectionStartDate { get; set; }
    [Display(Name = "تاریخ بازرسی")]
    [NotMapped]
    public string? InspectionStartDateDto { get; set; }

    [Display(Name = "تاریخ روانکاری")]
    public DateTime? LubricationStartDate { get; set; }

    [Display(Name = "تاریخ روانکاری")]
    [NotMapped]
    public string? LubricationStartDateDto { get; set; }

    [NotMapped]
    public IEnumerable<SparePartMachine> SparePartMachines { get; set; }
 
    [NotMapped]
    public IEnumerable<SparePart> SpareParts { get; set; }

    public GeneralStatus GeneralStatusId { get; set; } = GeneralStatus.Active;
}