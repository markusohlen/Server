namespace Application.Models;

public class MealView
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public List<InstructionStepView> Instructions { get; set; } = default!;
    public List<IngredientsView> Ingredients { get; set; } = default!;
    public int Portions { get; set; }
    public int TotalCalories { get; set; }
    public int TotalProtein { get; set; }
}
