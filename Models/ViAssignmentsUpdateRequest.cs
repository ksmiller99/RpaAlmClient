namespace RpaAlmClient.Models;

public class ViAssignmentsUpdateRequest
{
    public int? VirtualIdentityID { get; set; }
    public int? AutomationEnvironmentID { get; set; }
    public int? Percentage { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
