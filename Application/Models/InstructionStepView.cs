namespace Application.Models;

public class InstructionStepView
{
    public Guid Id { get; set; }
    public int StepNumber { get; set; }
    public string Text { get; set; } = default!;
    public List<InstructionIngredientView> Ingredients { get; set; } = default!;

    /// <summary>
    /// Renders the instruction text by replacing {0}, {1}, ... placeholders
    /// with "Amount Measure Name" from the referenced ingredients.
    /// Example: "Add {0} to a bowl" → "Add 5 Deciliter Mjöl to a bowl"
    /// </summary>
    public string DisplayText => Ingredients?.Count > 0
        ? string.Format(Text, Ingredients.Select(i => (object)$"{i.Amount} {i.Measure} {i.Name}").ToArray())
        : Text;
}
