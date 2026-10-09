using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToAvansToGo.Domain.Entities;

namespace ToAvansToGo.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.HasAlcohol).IsRequired();

        builder.Property(p => p.Picture)
            .IsRequired()
            .HasMaxLength(500);
        builder.HasData(
            new { Id = 1, Name = "Broodje kaas",  HasAlcohol = false, Picture = "broodje-kaas.jpg" },
            new { Id = 2, Name = "Tosti ham-kaas", HasAlcohol = false, Picture = "tosti.jpg" },
            new { Id = 3, Name = "Soep van de dag", HasAlcohol = false, Picture = "soep.jpg" },
            new { Id = 4, Name = "Fles bier",     HasAlcohol = true,  Picture = "bier.jpg" },
            new { Id = 5, Name = "Cola",          HasAlcohol = false, Picture = "cola.jpg" },
            new { Id = 6, Name = "Red Bull",          HasAlcohol = false, Picture = "redbull.jpg"},
            new { Id = 7, Name = "Croissant",          HasAlcohol = false, Picture = "croissant.jpg"}
        );
    }
}