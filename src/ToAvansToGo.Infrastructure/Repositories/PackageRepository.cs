using Microsoft.EntityFrameworkCore;
using ToAvansToGo.Application.InterFaces;
using ToAvansToGo.Domain.Entities;
using ToAvansToGo.Infrastructure.Data;

namespace ToAvansToGo.Infrastructure.Repositories;

public class PackageRepository(ApplicationDbContext context) : IPackageRepository
{
    public async Task<List<Package>> GetPackages()
    {
        return await context.Packages
            .AsNoTracking()
            .Include(p => p.Products)
            .Include(p => p.PickUpLocation)
            .Include(p => p.ReservedBy)
            .ToListAsync();
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        return await context.Products.AsNoTracking().ToListAsync();
    }

    public async Task<Package?> GetPackageByIdAsync(int id)
    {
        return await context.Packages
            .Include(p => p.Products)
            .Include(p => p.PickUpLocation)
            .Include(p => p.ReservedBy)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddPackageAsync(Package package)
    {
        // Bestaande producten, kantine en student mogen niet opnieuw als nieuwe rij ingevoegd worden
        package.Products = await ResolveProductsAsync(package.Products);
        

        context.Packages.Add(package);
        await context.SaveChangesAsync();
    }

    public async Task UpdatePackageAsync(Package package)
    {
        var existing = await context.Packages
                           .Include(p => p.Products)
                           .FirstOrDefaultAsync(p => p.Id == package.Id)
                       ?? throw new KeyNotFoundException($"Package with id {package.Id} was not found.");

        // Eerst de producten bepalen, want 'existing' en 'package' kunnen dezelfde instantie zijn
        var products = await ResolveProductsAsync(package.Products);

        context.Entry(existing).CurrentValues.SetValues(package); 

        existing.Products.Clear();
        existing.Products.AddRange(products);

        await context.SaveChangesAsync();
    }

    public async Task DeletePackageAsync(Package package)
    {
        context.Packages.Remove(package);
        await context.SaveChangesAsync();
    }

    private async Task<List<Product>> ResolveProductsAsync(IEnumerable<Product> products)
    {
        var ids = products.Select(p => p.Id).ToList();
        return await context.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
    }
}
