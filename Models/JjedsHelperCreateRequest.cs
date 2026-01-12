using System.ComponentModel.DataAnnotations;

namespace RpaAlmClient.Models;

public class JjedsHelperCreateRequest
{
    [Required(ErrorMessage = "WWID is required")]
    [StringLength(9, ErrorMessage = "WWID must be 9 characters")]
    public string Wwid { get; set; } = string.Empty;

    [StringLength(255)]
    public string? CommonName { get; set; }

    [StringLength(255)]
    [EmailAddress]
    public string? Email { get; set; }

    public DateTime? JjedsCreated { get; set; }

    public DateTime? JjedsLastChanged { get; set; }
}
