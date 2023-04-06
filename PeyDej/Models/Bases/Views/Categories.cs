using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Bases.Views;

[Table(name: "Categories", Schema = "Base")]
public class Categories
{
    public long CategoryId { get; set; }

    public string? CategoryCaption { get; set; }
    
    [Key] public long SubCategoryId { get; set; }

    public string SubCategoryCaption { get; set; } = null!;
}