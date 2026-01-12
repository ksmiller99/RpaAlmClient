namespace RpaAlmClient.Models;

public class AutomationEnvironmentDto
{
    public int Id { get; set; }
    public int? AutomationID { get; set; }
    public string? AppID { get; set; }
    public int? EnvironmentTypeID { get; set; }
}
