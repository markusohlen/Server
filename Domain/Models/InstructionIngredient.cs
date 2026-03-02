using Domain.Models.Enums;

namespace Domain.Models;

public class InstructionIngredient
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public Measure Measure { get; set; }
    public int SortOrder { get; set; }
    public Guid InstructionStepId { get; set; }
    public Guid IngredientId { get; set; }
    public Guid? SectionId { get; set; }
    public Ingredient Ingredient { get; set; } = default!;
}
