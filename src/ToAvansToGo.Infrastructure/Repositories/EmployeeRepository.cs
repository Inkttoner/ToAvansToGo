using Microsoft.EntityFrameworkCore;
using ToAvansToGo.Application.InterFaces;
using ToAvansToGo.Domain.Entities;
using ToAvansToGo.Infrastructure.Data;

namespace ToAvansToGo.Infrastructure.Repositories;

public class EmployeeRepository(ApplicationDbContext context) : IEmployeeRepository
{
    public async Task<Employee> GetEmployeeByIdAsync(int id)
    {
        return await context.Employees
                   .Include(e => e.Location)
                   .FirstOrDefaultAsync(e => e.Id == id)
               ?? throw new KeyNotFoundException($"Employee with id {id} was not found.");
    }

    public async Task AddEmployeeAsync(Employee employee)
    {
        context.Employees.Add(employee);
        await context.SaveChangesAsync();
    }

    public async Task UpdateEmployeeAsync(Employee employee)
    {
        // Alleen de employee zelf markeren, niet ook de gekoppelde Canteen
        context.Entry(employee).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task DeleteEmployeeAsync(Employee employee)
    {
        context.Employees.Remove(employee);
        await context.SaveChangesAsync();
    }
}
