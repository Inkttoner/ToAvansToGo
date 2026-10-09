using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToAvansToGo.Domain.Entities;

namespace ToAvansToGo.Infrastructure.Data.Configurations;

public class PackageConfiguration : IEntityTypeConfiguration<Package>
{
    public void Configure(EntityTypeBuilder<Package> builder)
    {
        builder.HasKey(p => p.Id);
 
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);
 
        builder.Property(p => p.TypeOfMeal)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
 
        builder.Property(p => p.PickUpTime).IsRequired();
        builder.Property(p => p.AvailableTill).IsRequired();
        builder.Property(p => p.Is18Plus).IsRequired();
        builder.Property(p => p.Price).IsRequired();
 
        // Package -> Canteen 
        builder.HasOne(p => p.PickUpLocation)
            .WithMany()
            .HasForeignKey(p => p.PickUpLocationId)
            .OnDelete(DeleteBehavior.Restrict);
 
        // Package <-> Product (many-to-many met koppeltabel)
        builder.HasMany(p => p.Products)
            .WithMany(p => p.Packages)
            .UsingEntity(j => j.ToTable("PackageProducts"));
    }
}