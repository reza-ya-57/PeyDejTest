using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Bases;

[Table(name:"SparePartMachine",Schema = "Base")]
public class SparePartMachine
{
    public SparePartMachine()
    {

    }
    public SparePartMachine(long machineId, long sparePartId) : this()
    {
        Id = 0;
        InsDate = DateTime.Now;
        MachineId = machineId;
        SparePartId = sparePartId;
    }
    public long Id { get; set; }

    public DateTime InsDate { get; set; }

    public long SparePartId { get; set; }
    
    public long MachineId { get; set; }



    [NotMapped]
    public virtual Machine Machine { get; set; } = null!;
    [NotMapped]
    public virtual SparePart SpareIpartdNavigation { get; set; } = null!;
}
