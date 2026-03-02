using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.EfCore.Context;

public class MealPlannerDbContext : DbContext
{
    public DbSet<Meal> Meals { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<InstructionStep> InstructionSteps { get; set; }
    public DbSet<InstructionIngredient> InstructionIngredients { get; set; }

    public MealPlannerDbContext(DbContextOptions<MealPlannerDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Measure enum to be stored as a string
        modelBuilder.Entity<Ingredient>()
            .Property(i => i.Measure)
            .HasConversion<string>();

        // Define the relationship between Meal and Ingredient
        modelBuilder.Entity<Meal>()
            .HasMany(m => m.Ingredients)
            .WithOne()
            .HasForeignKey(i => i.MealId);

        // Define the relationship between Meal and InstructionStep
        modelBuilder.Entity<Meal>()
            .HasMany(m => m.Instructions)
            .WithOne()
            .HasForeignKey(s => s.MealId);

        // Define the relationship between InstructionStep and InstructionIngredient
        modelBuilder.Entity<InstructionStep>()
            .HasMany(s => s.InstructionIngredients)
            .WithOne()
            .HasForeignKey(ii => ii.InstructionStepId);

        // Define the relationship between InstructionIngredient and Ingredient
        modelBuilder.Entity<InstructionIngredient>()
            .HasOne(ii => ii.Ingredient)
            .WithMany()
            .HasForeignKey(ii => ii.IngredientId);

        // Store Measure enum as string in InstructionIngredient
        modelBuilder.Entity<InstructionIngredient>()
            .Property(ii => ii.Measure)
            .HasConversion<string>();
    }
}