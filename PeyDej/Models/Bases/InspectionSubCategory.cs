using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Bases;

[Table(name: "InspectionSubCategory", Schema = "Base")]
public class InspectionSubCategory
{
    [Key] public long Id { get; set; }

    public DateTime InsDate { get; set; } = DateTime.Now;

    [Display(Name = "عنوان")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public string Caption { get; set; } = null!;

    [Display(Name = "دسته بازرسی")]
    [Range(typeof(long), "0", "65536", ErrorMessage = "مقدار {0} باید بین {1} تا این {2} باشد")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public long? InspectionCategoryId { get; set; }

    [Display(Name = "چرخه بازرسی")]
    [Range(typeof(long), "0", "65536", ErrorMessage = "مقدار {0} باید بین {1} تا این {2} باشد")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public long? InspectionCycle { get; set; }

    public GeneralStatus GeneralStatusId { get; set; } = GeneralStatus.Active;

    public virtual InspectionCategory? InspectionCategory { get; set; }
}