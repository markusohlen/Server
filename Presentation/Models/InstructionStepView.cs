namespace Presentation.Models;

public class InstructionStepView
{
    public Guid Id { get; set; }
    public int StepNumber { get; set; }
    public string Text { get; set; } = default!;
    public List<InstructionIngredientView> Ingredients { get; set; } = new();

    public string DisplayText => Ingredients?.Count > 0
        ? string.Format(Text, Ingredients.Select(i => (object)$"{i.Amount} {i.Measure} {i.Name}").ToArray())
        : Text;
}
