using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class CreateIngredientModel
{
    [Required(ErrorMessage = "Ingredient name is required.")]
    public string Name { get; set; } = string.Empty;

    public int? Calories { get; set; }

    public int? Protein { get; set; }

    public decimal? Price { get; set; }

    public int Volume { get; set; }

    public int Measure { get; set; }

    public bool LookingUp { get; set; }
}
