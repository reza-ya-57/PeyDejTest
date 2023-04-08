using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PeyDej.Models.Bases;

namespace PeyDej.Models.Inspection;

[Table(name: "MotorIS", Schema = "Inspection")]
public class MotorIS
{
  [Key] public long Id { get; set; }

  public DateTime InsDate { get; set; } = DateTime.Now;

  [Display(Name = "نام موتور")]
  [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
  [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
  public long? MotorId { get; set; }

  [Display(Name = "تاریخ بازرسی")]
  [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
  public DateTime InspectionDate { get; set; }

  [Display(Name = "تاریخ پایان بازرسی")]
  public DateTime? InspectionFinishedDate { get; set; }

  [Display(Name = "وضعیت بازرسی")]
  [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
  public InspectionStatus Status { get; set; } = InspectionStatus.NotOk;

  [NotMapped]
  public virtual IEnumerable<Motor> Motors { get; } = new List<Motor>();
}