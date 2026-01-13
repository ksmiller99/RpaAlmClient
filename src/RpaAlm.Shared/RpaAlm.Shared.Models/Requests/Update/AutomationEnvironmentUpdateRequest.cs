using System.ComponentModel.DataAnnotations;

namespace RpaAlm.Shared.Models.Requests.Update;

public class AutomationEnvironmentUpdateRequest
{
    public int? AutomationID { get; set; }

    [StringLength(10)]
    public string? AppID { get; set; }

    public int? EnvironmentTypeID { get; set; }
}
