using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Bases;

[Table(name: "Motor", Schema = "Base")]
public class Motor
{
    public Motor()
    {
        SpareParts = new List<SparePart>();
    }
    [Key]
    public long Id { get; set; }

    public DateTime InsDate { get; set; } = DateTime.Now;

    [Display(Name = "نام موتور")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public string Name { get; set; } = null!;

    [Display(Name = "شماره سریال")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public string SerialNumber { get; set; } = null!;

    [Display(Name = "محل نصب")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? Emplacement { get; set; }

    [Display(Name = "سازنده")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? Manufacturer { get; set; }

    [Display(Name = "توان مصرفی(KW)")]
    [Range(typeof(long), "0", "65536", ErrorMessage = "مقدار {0} باید بین {1} تا این {2} باشد")]
    public long? Kw { get; set; }

    [Display(Name = "ولتاژ مصرفی(V)")]
    [Range(typeof(long), "0", "65536", ErrorMessage = "مقدار {0} باید بین {1} تا این {2} باشد")]
    public long? V { get; set; }

    [Display(Name = "شماره تسمه")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? BeltSerial { get; set; }

    [Display(Name = "تعداد تسمه")]
    [Range(typeof(int), "0", "65536", ErrorMessage = "مقدار {0} باید بین {1} تا این {2} باشد")]
    public int? BeltCount { get; set; }

    [Display(Name = "فولی")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? Fooli { get; set; }

    [Display(Name = "شماره زنجیر")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? ChainSerial { get; set; }

    [Display(Name = "نوع")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? Type { get; set; }

    [Display(Name = "دنده")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? Gear { get; set; }

    [Display(Name = "چرخش بازرسی")]
    [Range(typeof(long), "0", "65536", ErrorMessage = "مقدار {0} باید بین {1} تا این {2} باشد")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public long? InspectionCycle { get; set; }

    [Display(Name = "توضیحات")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? Description { get; set; }

    [Display(Name = "قطعات")]
    [NotMapped]
    public List<long> SparePartIds { get; set; }


    [Display(Name = "تاریخ بازرسی")]
    public DateTime? InspectionStartDate { get; set; }
    [Display(Name = "تاریخ بازرسی")]
    [NotMapped]
    public string? InspectionStartDateDto { get; set; }

    [NotMapped]
    public IEnumerable<SparePart> SpareParts { get; set; }

    public GeneralStatus GeneralStatusId { get; set; } = GeneralStatus.Active;


}
