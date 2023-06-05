﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Bases;

[Table(name: "SubCategory", Schema = "Base")]
public class SubCategory
{
  public long Id { get; set; }

  public DateTime InsDate { get; set; }

  [Display(Name = "نام")]
  [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
  [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
  public string Name { get; set; } = null!;

  public long? CategoryId { get; set; }
  [Display(Name = "مقدار")]
  [MaxLength(1024, ErrorMessage = "مقدار {0} باید حداکثر {1} کاراکتر باشد")]
  [Required(ErrorMessage = "مقدار {0} را وارد کنید")]
  public string? Value { get; set; }

  public virtual Category? Category { get; set; }
}
