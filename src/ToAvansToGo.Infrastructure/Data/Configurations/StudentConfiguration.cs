using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToAvansToGo.Domain.Entities;
using ToAvansToGo.Domain.Enums;

namespace ToAvansToGo.Infrastructure.Data.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.StudentNumber).IsRequired();
        builder.HasIndex(s => s.StudentNumber).IsUnique();

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.DateOfBirth)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(s => s.Email)
            .IsRequired()
            .HasMaxLength(256);
        builder.HasIndex(s => s.Email).IsUnique();

        builder.Property(s => s.StudyCity)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(s => s.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.HasData(
            new { Id = 1, StudentNumber = 2200001, Name = "Jan Jansen",
                DateOfBirth = new DateTime(2003, 5, 14), Email = "jan.jansen@student.avans.nl",
                StudyCity = City.Breda, PhoneNumber = "0612345678" },
            new { Id = 2, StudentNumber = 2200002, Name = "Lisa de Vries",
                DateOfBirth = new DateTime(2008, 9, 2), Email = "lisa.devries@student.avans.nl",
                StudyCity = City.Tilburg, PhoneNumber = "0687654321" }
        );
    }
}