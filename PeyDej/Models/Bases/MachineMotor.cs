using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Bases
{

    [Table("MachineMotor", Schema = "Base")]
    public class MachineMotor
    {
        public MachineMotor(long machineId, long motorId) : this()
        {
            MachineId = machineId;
            MotorId = motorId;
        }

        public MachineMotor()
        {
            Id = 0;
            InsDate = DateTime.Now;
        }

        public long Id { get; set; }
        public DateTime InsDate { get; set; }
        public long MotorId { get; set; }
        [NotMapped]
        public List<long> MotorIds { get; set; }
        public long MachineId { get; set; }
    }

}
