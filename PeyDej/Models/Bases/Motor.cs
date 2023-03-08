using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Bases;

[Table(name:"Motor",Schema = "Base")]
public class Motor
{
    [Key]
    public long Id { get; set; }

    public DateTime InsDate { get; set; }

    /// <summary>
    /// شماره سریال
    /// </summary>
    public string SerialNumber { get; set; } = null!;

    /// <summary>
    /// محل نصب
    /// </summary>
    public string? Emplacement { get; set; }

    /// <summary>
    /// سازنده
    /// </summary>
    public string? Manufacturer { get; set; }

    public long? Kw { get; set; }

    public long? V { get; set; }

    /// <summary>
    /// شماره تسمه
    /// </summary>
    public string? BeltSerial { get; set; }

    /// <summary>
    /// تعداد
    /// </summary>
    public int? BeltCount { get; set; }

    public string? Fooli { get; set; }

    /// <summary>
    /// شماره زنجیر
    /// </summary>
    public string? ChainSerial { get; set; }

    /// <summary>
    /// نوع
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// دنده
    /// </summary>
    public string? Gear { get; set; }

    public long? MachineId { get; set; }

    public long? InspectionCycle { get; set; }

    /// <summary>
    /// توضیحات
    /// </summary>
    public string? Description { get; set; }

    public int GeneralStatusId { get; set; } = 1;
}
