using Domain.Models.Enums;

namespace Domain.Models;

public class Ingredient
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int Calories { get; set; }
    public int Protein { get; set; }
    public decimal Price { get; set; }
    public int Volume { get; set; }
    public Measure Measure { get; set; }
    public Guid MealId { get; set; }
}