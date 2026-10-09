using Microsoft.EntityFrameworkCore;
using ToAvansToGo.Application.InterFaces;
using ToAvansToGo.Domain.Entities;
using ToAvansToGo.Infrastructure.Data;

namespace ToAvansToGo.Infrastructure.Repositories;

public class CanteenRepository(ApplicationDbContext context) : ICanteenRepository
{
    public async Task<Canteen> GetCanteenByIdAsync(int id)
    {
        return await context.Canteens.FindAsync(id)
               ?? throw new KeyNotFoundException($"Canteen with id {id} was not found.");
    }

    public async Task<Canteen> GetCanteenByEmployeeIdAsync(int employeeId)
    {
        return await context.Employees
                   .Where(e => e.Id == employeeId)
                   .Select(e => e.Location)
                   .FirstOrDefaultAsync()
               ?? throw new KeyNotFoundException($"No canteen found for employee with id {employeeId}.");
    }

    public async Task AddCanteenAsync(Canteen canteen)
    {
        context.Canteens.Add(canteen);
        await context.SaveChangesAsync();
    }

    public async Task UpdateCanteenAsync(Canteen canteen)
    {
        context.Canteens.Update(canteen);
        await context.SaveChangesAsync();
    }

    public async Task DeleteCanteenAsync(Canteen canteen)
    {
        context.Canteens.Remove(canteen);
        await context.SaveChangesAsync();
    }
}
