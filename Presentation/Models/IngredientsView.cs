namespace Presentation.Models;

public class IngredientsView
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public int Calories { get; set; }
    public int Protein { get; set; }
    public decimal Price { get; set; }
    public int Volume { get; set; }
    public string Measure { get; set; } = default!;
}
