using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using PeyDej.Models.Bases;

namespace PeyDej.Models.Dtos;

public class InspectionCriteriaSubCategoryIS
{
    [Display(Name = "نام")]
    public long Id { get; set; }

    [Display(Name = "نام")]
    public string Caption { get; set; }

    [Display(Name = "تاریخ بازرسی")]
    public DateTime InspectionDate { get; set; }
    [Display(Name = "نام")]
    public long InspectionCriteriaCategoryId { get; set; }
    public long CriteriaCategoryId { get; set; }
    public List<SubCategory> SubCategories { get; set; }
}
