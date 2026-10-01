using ToAvansToGo.Application.InterFaces;
using ToAvansToGo.Domain.Entities;
using ToAvansToGo.Domain.Enums;

namespace ToAvansToGo.Application.Services;

public class PackageService: IPackageService
{
    IPackageRepository _packageRepository;
    IEmployeeRepository _employeeRepository;
    public PackageService(IPackageRepository packageRepository,  IEmployeeRepository employeeRepository)
    {
        _packageRepository = packageRepository;
        _employeeRepository = employeeRepository;
    }
    public async Task<List<Package>> GetPackagesAsync(City city, MealType? mealType)
    {
        var packages = await _packageRepository.GetPackages();
        var result = packages.Where(p => p.PickUpLocation.City == city);

        if (mealType != null)
        {
            result = result.Where(p => p.TypeOfMeal == mealType);
        }
        return result.OrderBy(p => p.PickUpTime).ToList();
    }

    public Task<List<Package>> GetPackagesReservedByStudentAsync(int studentId)
    {
        return  _packageRepository.GetPackagesReservedByStudentAsync(studentId);
    }

    public async Task<List<Package>> GetPackagesForCanteenAsync(bool isOwnCanteen, int canteenId)
    {
        var packages = await _packageRepository.GetPackages();
        
        if (isOwnCanteen)
        {
             packages = packages.Where(p => p.PickUpLocation.Id == canteenId).ToList();
        }
        else
        {
             packages  = packages.Where(p => p.PickUpLocation.Id != canteenId).ToList();
        }

        return packages.OrderBy(p => p.PickUpTime).ToList();
    }
    
    
    public Task<Package?> GetPackageByIdAsync(int id)
    {
        return _packageRepository.GetPackageByIdAsync(id);
    }

    public async Task<List<Product>> GetProductHistoryAsync(MealType mealType)
    {
        var packages = await _packageRepository.GetPackages();
        var filteredPackages = packages.Where(p => p.TypeOfMeal == mealType);
        return filteredPackages.SelectMany(p => p.Products).GroupBy(p => p.Id).OrderByDescending(g => g.Count()).Select(g => g.First()).Take(5).ToList();
    }

    public async Task CreatePackageAsync(Package package, int employeeId)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
        if ((package.PickUpTime.Date - DateTime.Today).Days >2)
        {
            throw new InvalidOperationException("A package can not be created more than 2 days in advance");
        }

        package.PickUpLocation = employee.Location;
        package.Is18Plus = package.Products.Any(p => p.HasAlcohol);
       await _packageRepository.AddPackageAsync(package);
    }

    public Task UpdatePackageAsync(Package package)
    {
        if (package.ReservedBy != null)
            throw new InvalidOperationException("Package is reserved and can not be edited");
        
        package.Is18Plus = package.Products.Any(p => p.HasAlcohol);
        return _packageRepository.UpdatePackageAsync(package);
    }

    public Task DeletePackageAsync(Package package)
    {
        if (package.ReservedBy != null)
            throw new InvalidOperationException("Package is reserved and can not be deleted");
        return _packageRepository.DeletePackageAsync(package);
    }
}