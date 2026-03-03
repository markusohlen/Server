namespace Domain.Models;

public class Meal
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public List<string> Categories { get; set; } = [];
    public List<InstructionStep> Instructions { get; set; } = default!;
    public List<Ingredient> Ingredients { get; set; } = default!;
    public int Portions { get; set; }
}
