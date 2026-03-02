using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.EfCore.Context;

public class MealPlannerDbContext : DbContext
{
    public DbSet<Meal> Meals { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<InstructionStep> InstructionSteps { get; set; }
    public DbSet<InstructionIngredient> InstructionIngredients { get; set; }

    public MealPlannerDbContext(DbContextOptions<MealPlannerDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MealPlannerDbContext).Assembly);
    }
}