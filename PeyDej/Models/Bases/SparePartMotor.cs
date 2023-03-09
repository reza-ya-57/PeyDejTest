using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Bases;

[Table(name:"SparePartMotor",Schema = "Base")]
public class SparePartMotor
{
    public long Id { get; set; }

    public DateTime InsDate { get; set; }

    public long SparePartId { get; set; }

    public long MotorId { get; set; }

    public virtual ICollection<SparePartMotor> InverseSparePart { get; } = new List<SparePartMotor>();

    public virtual Motor Motor { get; set; } = null!;

    public virtual SparePartMotor SparePart { get; set; } = null!;
}
