using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeyDej.Models.Report;

[Table(name: "LoadingReportDetail", Schema = "Report")]
public class LoadingReportDetail
{
    [Key] public long Id { get; set; }

    public DateTime InsDate { get; set; } = DateTime.Now;
    [Display(Name = "توع تیغه")]
    public long BladKindId { get; set; }
    public long? LoadingReportId { get; set; }
    [Display(Name ="مقدار")]
    public long? Value { get; set; }
}
