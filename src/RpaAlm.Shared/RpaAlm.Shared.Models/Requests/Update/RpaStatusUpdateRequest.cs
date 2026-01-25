namespace RpaAlm.Shared.Models.Requests.Update;

public class RpaStatusUpdateRequest
{
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
}
