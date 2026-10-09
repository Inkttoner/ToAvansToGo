using Microsoft.EntityFrameworkCore;
using ToAvansToGo.Domain.Entities;

namespace ToAvansToGo.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Canteen> Canteens => Set<Canteen>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Package> Packages => Set<Package>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Student> Students => Set<Student>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}