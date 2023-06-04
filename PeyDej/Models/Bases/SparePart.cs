using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Bases;

[Table(name: "SparePart", Schema = "Base")]
public class SparePart
{
    [Key] public long Id { get; set; }

    public DateTime InsDate { get; set; } = DateTime.Now;

    [Display(Name = "نام قطعه")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public string Name { get; set; } = null!;

    [Display(Name = "مدل")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? Model { get; set; }

    //// [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    //[Display(Name = "تعداد")]
    //[Range(typeof(long), "0", "65536", ErrorMessage = "مقدار {0} باید بین {1} تا این {2} باشد")]
    //public long? Count { get; set; }

    //[Display(Name = "محل نصب")]
    //[MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    //public string? Emplacement { get; set; }

    [Display(Name = "توضیحات")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? Description { get; set; }

    public Guid? FileId { get; set; }
    public virtual IEnumerable<SparePartMachine> SparePartMachines { get; } = new List<SparePartMachine>();

    public GeneralStatus GeneralStatusId { get; set; } = GeneralStatus.Active;
    [NotMapped]
    public IFormFile? FormFiles { get; set; }
    public string? CreatorUserId { get; set; }
    public string? LastEditorUserId { get; set; }

    public DateTime? LastEditDate { get; set; }
}