namespace Domain.Models;

public class InstructionStep
{
    public Guid Id { get; set; }
    public int StepNumber { get; set; }
    public string Text { get; set; } = default!;
    public Guid MealId { get; set; }
    public Guid? SectionId { get; set; }
    public List<InstructionIngredient> InstructionIngredients { get; set; } = default!;
}
