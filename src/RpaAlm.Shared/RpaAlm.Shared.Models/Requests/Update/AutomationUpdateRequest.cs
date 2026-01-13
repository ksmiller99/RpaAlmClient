using System.ComponentModel.DataAnnotations;

namespace RpaAlm.Shared.Models.Requests.Update;

public class AutomationUpdateRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    public int? SegmentID { get; set; }

    public int? RegionID { get; set; }

    public int? FunctionID { get; set; }

    public int? StatusID { get; set; }

    [StringLength(9, ErrorMessage = "BtoWWID must be 9 characters")]
    public string? BtoWWID { get; set; }

    [StringLength(9, ErrorMessage = "BoWWID must be 9 characters")]
    public string? BoWWID { get; set; }

    [StringLength(9, ErrorMessage = "FcWWID must be 9 characters")]
    public string? FcWWID { get; set; }

    [StringLength(10)]
    public string? BuildZcode { get; set; }

    [StringLength(10)]
    public string? BuildCostCenter { get; set; }

    [StringLength(9, ErrorMessage = "SseWWID must be 9 characters")]
    public string? SseWWID { get; set; }

    [StringLength(9, ErrorMessage = "LseWWID must be 9 characters")]
    public string? LseWWID { get; set; }
}
