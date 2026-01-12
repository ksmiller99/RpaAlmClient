namespace RpaAlmClient.Models;

public class EnhancementUserStoryDto
{
    public int Id { get; set; }
    public int? EnhancementID { get; set; }
    public string? JiraIssue { get; set; }
    public int? StoryPoints { get; set; }
    public string? JiraIssueLink { get; set; }
    public string? JiraIssueSummary { get; set; }
    public int? StoryPointCostID { get; set; }
}
