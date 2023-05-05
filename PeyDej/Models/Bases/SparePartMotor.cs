using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Bases;

[Table(name:"SparePartMotor",Schema = "Base")]
public class SparePartMotor
{
    public SparePartMotor()
    {

    }
    public SparePartMotor(long motorId, long sparePartId) : this()
    {
        Id = 0;
        InsDate = DateTime.Now;
        MotorId = motorId;
        SparePartId = sparePartId;
    }
    public long Id { get; set; }

    public DateTime InsDate { get; set; }

    public long SparePartId { get; set; }
    [NotMapped]
    [Display(Name = "نام قطعه")]
    public List<string> SparePartIds { get; set; }

    public long MotorId { get; set; }

    public virtual IEnumerable<SparePartMotor> InverseSparePart { get; } = new List<SparePartMotor>();

    public virtual Motor Motor { get; set; } = null!;

    public virtual SparePartMotor SparePart { get; set; } = null!;
}
