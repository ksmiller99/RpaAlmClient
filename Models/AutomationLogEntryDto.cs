namespace RpaAlmClient.Models;

public class AutomationLogEntryDto
{
    public int Id { get; set; }
    public int? AutomationID { get; set; }
    public string? CreatedWWID { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? Comment { get; set; }
}
