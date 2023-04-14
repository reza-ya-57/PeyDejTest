using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Inspection.Views;

[Table(name: "vwInspectionSubCategoryIS", Schema = "Inspection")]
public class VwInspectionSubCategoryIS
{
    [Key] 
    public long InspectionSubCategoryIsId { get; set; }

    public long InspectionSubCategoryId { get; set; }

    [Display(Name = "تاریخ بارزسی")] 
    public DateTime InspectionDate { get; set; }

    [Display(Name = "تاریخ پایان بارزسی")] 
    public DateTime? InspectionFinishedDate { get; set; }

    [Display(Name = "وضعیت")] 
    public bool Status { get; set; }

    [Display(Name = "نام بارزسی")] 
    public string Caption { get; set; } = null!;

    public long? InspectionCategoryId { get; set; }
}