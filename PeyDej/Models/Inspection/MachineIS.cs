using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PeyDej.Models.Bases;

namespace PeyDej.Models.Inspection;

[Table(name: "MachineIS", Schema = "Inspection")]
public class MachineIS
{
    [Key] 
    public long Id { get; set; }

    public DateTime InsDate { get; set; } = DateTime.Now;

    public long MachineId { get; set; }

    /// <summary>
    /// the day that machine need to be inspected
    /// </summary>
    public DateTime InspectionDate { get; set; }

    /// <summary>
    /// save information of when inspection done and change status from 0 to 1 
    /// </summary>
    public DateTime? InspectionFinishedDate { get; set; }

    /// <summary>
    /// is machine inspected?
    /// </summary>
    public InspectionStatus Status { get; set; } = InspectionStatus.NotOk;

    public virtual IEnumerable<Machine> Motors { get; } = new List<Machine>();
}