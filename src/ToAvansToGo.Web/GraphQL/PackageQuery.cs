using HotChocolate;
using ToAvansToGo.Web.Dtos;
using ToAvansToGo.Application.InterFaces;
using ToAvansToGo.Domain.Enums;

namespace ToAvansToGo.Web.GraphQL;

public class PackageQuery
{
    public async Task<List<PackageDto>> GetPackages(
        [Service] IPackageService packageService,
        City city = City.Breda,
        MealType? mealType = null)
    {
        var packages = await packageService.GetPackagesAsync(city, mealType);

        return packages.Select(p => new PackageDto(
            p.Id, p.Name, p.PickUpTime, p.AvailableTill, p.Price, p.Is18Plus,
            p.TypeOfMeal.ToString(), p.PickUpLocation?.Location,
            p.Products.Select(x => new ProductDto(x.Id, x.Name, x.HasAlcohol, x.Picture)).ToList()
        )).ToList();
    }
}