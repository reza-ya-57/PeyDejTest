using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Bases;

[Table(name:"SubCategory",Schema = "Base")]
public class SubCategory
{
    public long Id { get; set; }

    public DateTime InsDate { get; set; }

    public long? CategoryId { get; set; }

    public string? Name { get; set; }

    public string Value { get; set; } = null!;

    public virtual Category? Category { get; set; }
}
