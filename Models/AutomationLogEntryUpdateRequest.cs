using System.ComponentModel.DataAnnotations;

namespace RpaAlmClient.Models;

public class AutomationLogEntryUpdateRequest
{
    public int? AutomationID { get; set; }

    [StringLength(9)]
    public string? CreatedWWID { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Comment { get; set; }
}
