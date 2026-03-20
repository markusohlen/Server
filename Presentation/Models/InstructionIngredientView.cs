namespace Presentation.Models;

public class InstructionIngredientView
{
    public Guid IngredientId { get; set; }
    public string Name { get; set; } = default!;
    public decimal Amount { get; set; }
    public string Measure { get; set; } = default!;
}
