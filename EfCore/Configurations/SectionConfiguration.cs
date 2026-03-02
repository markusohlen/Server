using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.EfCore.Configurations;

public class SectionConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        // Define the relationship between Section and InstructionStep
        builder.HasMany<InstructionStep>()
            .WithOne()
            .HasForeignKey(s => s.SectionId)
            .OnDelete(DeleteBehavior.SetNull);

        // Define the relationship between Section and InstructionIngredient
        builder.HasMany<InstructionIngredient>()
            .WithOne()
            .HasForeignKey(ii => ii.SectionId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
