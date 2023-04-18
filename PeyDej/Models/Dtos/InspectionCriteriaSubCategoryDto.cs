namespace PeyDej.Models.Dtos;

public class InspectionCriteriaSubCategoryDto
{
    public long InspectionId { get; set; }
    public long CriteriaId { get; set; }
    public long PersonId { get; set; }
    public int Status { get; set; }
    public string Description { get; set; }
}
