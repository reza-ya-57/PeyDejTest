using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Bases;

[Table(name:"SparePartMachine",Schema = "Base")]
public class SparePartMachine
{
    public long Id { get; set; }

    public DateTime InsDate { get; set; }

    public long SpareIpartd { get; set; }

    public long MachineId { get; set; }

    public virtual Machine Machine { get; set; } = null!;

    public virtual SparePart SpareIpartdNavigation { get; set; } = null!;
}
