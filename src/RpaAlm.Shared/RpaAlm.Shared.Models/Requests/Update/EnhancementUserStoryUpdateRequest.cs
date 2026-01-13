using System.ComponentModel.DataAnnotations;

namespace RpaAlm.Shared.Models.Requests.Update;

public class EnhancementUserStoryUpdateRequest
{
    public int? EnhancementID { get; set; }

    [StringLength(50)]
    public string? JiraIssue { get; set; }

    public int? StoryPoints { get; set; }

    [StringLength(500)]
    public string? JiraIssueLink { get; set; }

    [StringLength(500)]
    public string? JiraIssueSummary { get; set; }

    public int? StoryPointCostID { get; set; }
}
