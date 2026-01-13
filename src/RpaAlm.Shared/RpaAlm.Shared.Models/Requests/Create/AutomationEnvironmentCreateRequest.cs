using System.ComponentModel.DataAnnotations;

namespace RpaAlm.Shared.Models.Requests.Create;

public class AutomationEnvironmentCreateRequest
{
    public int? AutomationID { get; set; }

    [StringLength(10)]
    public string? AppID { get; set; }

    public int? EnvironmentTypeID { get; set; }
}
