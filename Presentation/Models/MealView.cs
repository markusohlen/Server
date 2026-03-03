namespace Presentation.Models;

public class MealView
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public List<InstructionStepView> Instructions { get; set; } = new();
    public List<IngredientsView> Ingredients { get; set; } = new();
    public int Portions { get; set; }
    public int TotalCalories { get; set; }
    public int TotalProtein { get; set; }
}
