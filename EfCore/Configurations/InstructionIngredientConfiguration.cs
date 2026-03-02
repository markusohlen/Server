using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EfCore.Configurations;

public class InstructionIngredientConfiguration : IEntityTypeConfiguration<InstructionIngredient>
{
    public void Configure(EntityTypeBuilder<InstructionIngredient> builder)
    {
        // Define the relationship between InstructionIngredient and Ingredient
        builder.HasOne(ii => ii.Ingredient)
            .WithMany()
            .HasForeignKey(ii => ii.IngredientId);

        // Store Measure enum as string in InstructionIngredient
        builder.Property(ii => ii.Measure)
            .HasConversion<string>();
    }
}
