namespace Application.Models;

public class AiRecipeImprovement
{
    public List<string> Suggestions { get; set; } = [];
    public List<AiIngredientAlternative> AlternativeIngredients { get; set; } = [];
    public List<string> Tips { get; set; } = [];
}

public class AiIngredientAlternative
{
    public string Original { get; set; } = string.Empty;
    public string Alternative { get; set; } = string.Empty;
}
