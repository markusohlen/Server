namespace Domain.Models;

public class Section
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public int SortOrder { get; set; }
    public Guid MealId { get; set; }
}
