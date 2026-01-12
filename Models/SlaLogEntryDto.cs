namespace RpaAlmClient.Models;

public class SlaLogEntryDto
{
    public int Id { get; set; }
    public int? SlaMasterID { get; set; }
    public string? CreatedWWID { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? Comment { get; set; }
}
