using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Bases;

[Table(name: "Category", Schema = "Base")]
public class Category
{
    [Key] public long Id { get; set; }

    public DateTime InsDate { get; set; } = DateTime.Now;

    public string Name { get; set; } = null!;

    public string? Value { get; set; }

    public virtual IEnumerable<SubCategory> SubCategories { get; } = new List<SubCategory>();
}