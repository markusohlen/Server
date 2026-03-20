namespace Application.Models;

public class ImportedRecipeData
{
    public string Name { get; set; } = string.Empty;
    public List<string> Categories { get; set; } = [];
    public int Portions { get; set; } = 1;
    public List<ImportedIngredient> Ingredients { get; set; } = [];
    public List<string> Instructions { get; set; } = [];
}
