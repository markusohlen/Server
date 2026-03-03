using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EfCore.Configurations;

public class MealConfiguration : IEntityTypeConfiguration<Meal>
{
    public void Configure(EntityTypeBuilder<Meal> builder)
    {

        // Define the relationship between Meal and Ingredient
        builder.HasMany(m => m.Ingredients)
            .WithOne()
            .HasForeignKey(i => i.MealId);

        // Define the relationship between Meal and Section
        builder.HasMany<Section>()
            .WithOne()
            .HasForeignKey(s => s.MealId);

        // Define the relationship between Meal and InstructionStep
        builder.HasMany(m => m.Instructions)
            .WithOne()
            .HasForeignKey(s => s.MealId);
    }
}
