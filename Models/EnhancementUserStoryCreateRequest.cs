using System.ComponentModel.DataAnnotations;

namespace RpaAlmClient.Models;

public class EnhancementUserStoryCreateRequest
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
