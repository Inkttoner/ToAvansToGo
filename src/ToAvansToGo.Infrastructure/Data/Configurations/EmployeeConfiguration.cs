using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToAvansToGo.Domain.Entities;

namespace ToAvansToGo.Infrastructure.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.EmployeeNumber).IsRequired();
        builder.HasIndex(e => e.EmployeeNumber).IsUnique();

        builder.HasOne(e => e.Location)
            .WithMany()
            .HasForeignKey(e => e.CanteenId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict); 
        
        builder.HasData(
            new { Id = 1, Name = "Piet Bakker", EmployeeNumber = 1001, CanteenId = 1 },
            new { Id = 2, Name = "Sanne Smit",  EmployeeNumber = 1002, CanteenId = 2 }
        );
    }
    
    
}