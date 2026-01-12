using System.ComponentModel.DataAnnotations;

namespace RpaAlmClient.Models;

public class SlaMasterUpdateRequest
{
    public int? AutomationID { get; set; }

    public int? ComplexityID { get; set; }

    public int? MedalID { get; set; }

    [StringLength(10)]
    public string? Zcode { get; set; }

    [StringLength(10)]
    public string? CostCenter { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
