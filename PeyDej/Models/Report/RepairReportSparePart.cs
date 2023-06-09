using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Report;

[Table(name: "RepairReportSparePart", Schema = "Report")]
public class RepairReportSparePart
{
    public long Id { get; set; }
    public long RepairReportId { get; set; }
    public long SparePartId { get; set; }
    public DateTime? ReceiveDate { get; set; }
    public int? ReceiveHour { get; set; }
    public int? Count { get; set; }
}
