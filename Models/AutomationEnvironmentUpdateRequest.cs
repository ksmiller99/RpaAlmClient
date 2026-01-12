using System.ComponentModel.DataAnnotations;

namespace RpaAlmClient.Models;

public class AutomationEnvironmentUpdateRequest
{
    public int? AutomationID { get; set; }

    [StringLength(10)]
    public string? AppID { get; set; }

    public int? EnvironmentTypeID { get; set; }
}
