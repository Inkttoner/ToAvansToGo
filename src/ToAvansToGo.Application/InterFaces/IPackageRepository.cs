using ToAvansToGo.Domain.Entities;

namespace ToAvansToGo.Application.InterFaces;

public interface IPackageRepository
{
    Task<List<Package>> GetPackages();
    Task<List<Product>> GetProductsAsync();
    Task<Package?> GetPackageByIdAsync(int id);
    Task UpdatePackageAsync(Package package); 
    Task AddPackageAsync(Package package);
    Task DeletePackageAsync(Package package);

}