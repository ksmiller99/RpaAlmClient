using System.ComponentModel.DataAnnotations;

namespace RpaAlm.Shared.Models.Requests.Update;

public class AutomationLogEntryUpdateRequest
{
    public int? AutomationID { get; set; }

    [StringLength(9)]
    public string? CreatedWWID { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Comment { get; set; }
}
