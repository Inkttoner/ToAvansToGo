using Microsoft.EntityFrameworkCore;
using ToAvansToGo.Application.InterFaces;
using ToAvansToGo.Domain.Entities;
using ToAvansToGo.Infrastructure.Data;

namespace ToAvansToGo.Infrastructure.Repositories;

public class ProductRepository(ApplicationDbContext context) : IProductRepository
{
    public async Task<List<Product>> GetProductsAsync()
    {
        return await context.Products.AsNoTracking().ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await context.Products.FindAsync(id)
               ?? throw new KeyNotFoundException($"Product with id {id} was not found.");
    }

    public async Task AddProductAsync(Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(Product product)
    {
        context.Entry(product).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(Product product)
    {
        context.Products.Remove(product);
        await context.SaveChangesAsync();
    }
}
