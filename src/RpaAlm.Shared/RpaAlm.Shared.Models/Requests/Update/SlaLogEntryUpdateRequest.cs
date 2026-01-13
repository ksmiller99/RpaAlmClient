using System.ComponentModel.DataAnnotations;

namespace RpaAlm.Shared.Models.Requests.Update;

public class SlaLogEntryUpdateRequest
{
    public int? SlaMasterID { get; set; }

    [StringLength(9)]
    public string? CreatedWWID { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Comment { get; set; }
}
