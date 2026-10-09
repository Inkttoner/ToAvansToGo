using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToAvansToGo.Domain.Entities;
using ToAvansToGo.Domain.Enums;

namespace ToAvansToGo.Infrastructure.Data.Configurations;

public class CanteenConfiguration : IEntityTypeConfiguration<Canteen>
{
    public void Configure(EntityTypeBuilder<Canteen> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.City)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.Location)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.HasHotMeals).IsRequired();

        // Dezelfde locatie in dezelfde stad mag maar één keer voorkomen
        builder.HasIndex(c => new { c.City, c.Location }).IsUnique();
        
        builder.HasData(
            new { Id = 1, City = City.Breda,   Location = "LA Gebouw",   HasHotMeals = true },
            new { Id = 2, City = City.Tilburg, Location = "Hoofdkantine", HasHotMeals = true },
            new { Id = 3, City = City.Denbosch, Location = "Kantine Onderwijsboulevard", HasHotMeals = false },
            new { Id = 4, City = City.Breda,   Location = "LD Gebouw",   HasHotMeals = true }
        );
    }
}