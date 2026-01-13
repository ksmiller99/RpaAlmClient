namespace RpaAlm.Shared.Models.DTOs;

public class AutomationDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? SegmentID { get; set; }
    public int? RegionID { get; set; }
    public int? FunctionID { get; set; }
    public int? StatusID { get; set; }
    public string? BtoWWID { get; set; }
    public string? BoWWID { get; set; }
    public string? FcWWID { get; set; }
    public string? BuildZcode { get; set; }
    public string? BuildCostCenter { get; set; }
    public string? SseWWID { get; set; }
    public string? LseWWID { get; set; }
}
