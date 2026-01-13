namespace RpaAlm.Shared.Models.DTOs;

public class SlaMasterDto
{
    public int Id { get; set; }
    public int? AutomationID { get; set; }
    public int? ComplexityID { get; set; }
    public int? MedalID { get; set; }
    public string? Zcode { get; set; }
    public string? CostCenter { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
