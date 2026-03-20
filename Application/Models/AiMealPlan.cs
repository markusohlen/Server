namespace Application.Models;

public class AiMealPlan
{
    public List<AiMealPlanDay> Days { get; set; } = [];
}

public class AiMealPlanDay
{
    public int Day { get; set; }
    public List<AiMealPlanEntry> Meals { get; set; } = [];
}

public class AiMealPlanEntry
{
    public string Type { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
