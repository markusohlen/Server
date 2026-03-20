namespace Application.Models;

public class AiMealSuggestion
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int EstimatedMinutes { get; set; }
    public List<string> Categories { get; set; } = [];
}
