using System.ComponentModel.DataAnnotations;

namespace RpaAlm.Shared.Models.Requests.Update;

public class VirtualIdentityUpdateRequest
{
    [StringLength(255)]
    public string? AccountName { get; set; }

    [StringLength(255)]
    public string? HostName { get; set; }

    [StringLength(9)]
    public string? WWID { get; set; }

    [StringLength(15)]
    public string? IPv4 { get; set; }

    public int? ADDomainID { get; set; }

    [StringLength(255)]
    [EmailAddress]
    public string? Email { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Retired { get; set; }
}
