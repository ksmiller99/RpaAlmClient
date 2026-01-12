using System.ComponentModel.DataAnnotations;

namespace RpaAlmClient.Models;

public class CmdbHelperUpdateRequest
{
    [StringLength(255)]
    public string? Name { get; set; }

    [StringLength(50)]
    public string? Zcode { get; set; }
}
