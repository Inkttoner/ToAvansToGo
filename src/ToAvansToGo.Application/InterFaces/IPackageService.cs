using ToAvansToGo.Domain.Entities;
using ToAvansToGo.Domain.Enums;

namespace ToAvansToGo.Application.InterFaces;

public interface IPackageService
{
    Task<List<Package>> GetPackagesAsync(City city, MealType? mealType);
    Task<List<Package>> GetPackagesReservedByStudentAsync(int studentId);
    Task<List<Package>> GetPackagesForCanteenAsync(bool isOwnCanteen ,int canteenId);
    Task<Package?> GetPackageByIdAsync(int id);
    Task<List<Product>> GetProductHistoryAsync(MealType mealType);
    
    Task CreatePackageAsync(Package package, int employeeId);
    Task UpdatePackageAsync(Package package);
    Task DeletePackageAsync(Package package);
}