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

    public string Caption { get; set; } = null!;

    public long? InspectionCategoryId { get; set; }

    public long? InspectionCycle { get; set; }

    public int GeneralStatusId { get; set; }

    public virtual InspectionCategory? InspectionCategory { get; set; }
}