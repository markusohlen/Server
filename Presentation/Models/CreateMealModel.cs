using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class CreateMealModel
{
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    public string Categories { get; set; } = string.Empty;

    [Range(1, 100, ErrorMessage = "Portions must be between 1 and 100.")]
    public int Portions { get; set; } = 1;

    public List<CreateIngredientModel> Ingredients { get; set; } = [new()];

    public List<CreateInstructionStepModel> Instructions { get; set; } = [new()];
}
