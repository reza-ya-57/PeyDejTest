using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Bases
{

    [Table("MachineMotor", Schema = "Base")]
    public class MachineMotor
    {
        public MachineMotor(long machineId, long motorId)
        {
            Id = 0;
            InsDate = DateTime.Now;
            MachineId = machineId;
            MotorId = motorId;
        }
        public long Id { get; set; }
        public DateTime InsDate { get; set; }
        public long MotorId { get; set; }
        public long MachineId { get; set; }
    }

}
