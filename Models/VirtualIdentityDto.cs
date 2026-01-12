namespace RpaAlmClient.Models;

public class VirtualIdentityDto
{
    public int Id { get; set; }
    public string? AccountName { get; set; }
    public string? HostName { get; set; }
    public string? WWID { get; set; }
    public string? IPv4 { get; set; }
    public int? ADDomainID { get; set; }
    public string? Email { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Retired { get; set; }
}
