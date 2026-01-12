using System.ComponentModel.DataAnnotations;

namespace RpaAlmClient.Models;

public class AutomationEnvironmentCreateRequest
{
    public int? AutomationID { get; set; }

    [StringLength(10)]
    public string? AppID { get; set; }

    public int? EnvironmentTypeID { get; set; }
}
