using System.ComponentModel.DataAnnotations;

namespace PeyDej.Models.ActiveModels;

public class MotorReport
{
    public long? Id { get; set; }
    
    [Display(Name = "نام موتور")]
    public string Name { get; set; }
    
    [Display(Name = "توضیحات")]
    public string Description { get; set; }
}