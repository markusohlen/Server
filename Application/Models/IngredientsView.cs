namespace Application.Models;

public class IngredientsView
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int Calories { get; set; }
    public int Protein { get; set; }
    public decimal Price { get; set; }
    public int Volume { get; set; }
    public required string Measure { get; set; }
}
