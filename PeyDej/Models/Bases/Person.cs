using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PeyDej.Attributes;
using PeyDej.Models.Bases.Views;

namespace PeyDej.Models.Bases;

[Table(name: "Person", Schema = "Base")]
public partial class Person
{
    [Key] 
    public long Id { get; set; }

    public DateTime InsDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    [Display(Name = "نام")]
    [MaxLength(64, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    [Display(Name = "نام خانوادگی")]
    [MaxLength(64, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string LastName { get; set; } = null!;

    [Display(Name = "کد ملی")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    [MaxLength(10, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? NationalCode { get; set; } = null!;

    [Display(Name = "شماره تلفن")]
    [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
    [MaxLength(64, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? PhoneNumber { get; set; } = null!;

    [Display(Name = "دپارتمان ها")]
    [NotZero(ErrorMessage = "مقدار {0} را وارد کنید")]
    public long? DepartmentId { get; set; }

    [Display(Name = "جنسیت")]
    [NotZero(ErrorMessage = "مقدار {0} را وارد کنید")]
    public long? GenderId { get; set; }

    [Display(Name = "توضیحات")]
    [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
    public string? Description { get; set; }

    [Display(Name = "بخش")]
    [NotZero(ErrorMessage = "مقدار {0} را وارد کنید")]
    public long? PartId { get; set; }

    [Display(Name = "واحد")]
    [NotZero(ErrorMessage = "مقدار {0} را وارد کنید")]
    public long? UnitId { get; set; }

    public GeneralStatus GeneralStatusId { get; set; } = GeneralStatus.Active;
}