using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EfCore.Configurations;

public class InstructionStepConfiguration : IEntityTypeConfiguration<InstructionStep>
{
    public void Configure(EntityTypeBuilder<InstructionStep> builder)
    {
        // Define the relationship between InstructionStep and InstructionIngredient
        builder.HasMany(s => s.InstructionIngredients)
            .WithOne()
            .HasForeignKey(ii => ii.InstructionStepId);
    }
}
