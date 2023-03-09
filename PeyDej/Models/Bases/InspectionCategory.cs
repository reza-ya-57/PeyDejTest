using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Bases;

[Table(name: "InspectionCategory", Schema = "Base")]
public class InspectionCategory
{
    [Key] public long Id { get; set; }

    public DateTime InsDate { get; set; } = DateTime.Now;
    
    [Display(Name = "عنوان")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    public string Caption { get; set; } = null!;

    public virtual IEnumerable<InspectionSubCategory> InspectionSubCategories { get; } =
        new List<InspectionSubCategory>();
}