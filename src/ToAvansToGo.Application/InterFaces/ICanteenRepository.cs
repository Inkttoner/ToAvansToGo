using ToAvansToGo.Domain.Entities;

namespace ToAvansToGo.Application.InterFaces;

public interface ICanteenRepository
{
    Task<Canteen> GetCanteenByIdAsync(int id);
    Task<Canteen> GetCanteenByEmployeeIdAsync(int employeeId);
    Task AddCanteenAsync(Canteen canteen);
    Task UpdateCanteenAsync(Canteen canteen);
    Task DeleteCanteenAsync(Canteen canteen);
}