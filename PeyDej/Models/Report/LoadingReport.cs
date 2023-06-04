using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Build.Framework;

namespace PeyDej.Models.Report;

[Table(name: "LoadingReport", Schema = "Report")]
public class LoadingReport
{
    [Key] 
    public long Id { get; set; }

    public DateTime InsDate { get; set; } = DateTime.Now;

    [Display(Name = "تاریخ")]
    [MaxLength(10, ErrorMessage = "مقدار {0} باید 10 کاراکتر باشد")]
    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "مقدار {0} الزامی می باشد")]
    public string Date { get; set; }

    /// <summary>
    /// SELECT * FROM  Base.SubCategory WHERE CategoryId = 8
    /// </summary>
    ///


    [Display(Name = "مجموع بارگیری")]
    public long? LoadingCount { get; set; }

    [Display(Name = "تعداد درگاه باز شده")]
    public long? OpenGateCount { get; set; }

    [Display(Name = "توضیحات")] 
    public string? Description { get; set; }

}