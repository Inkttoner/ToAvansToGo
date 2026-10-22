using ToAvansToGo.Domain.Entities;

namespace ToAvansToGo.Web.Dtos;

public static class PackageMapping
{
    public static PackageDto ToDto(this Package p) => new(
        p.Id, p.Name, p.PickUpTime, p.AvailableTill, p.Price, p.Is18Plus,
        p.TypeOfMeal.ToString(), p.PickUpLocation?.Location,
        p.Products.Select(x => new ProductDto(x.Id, x.Name, x.HasAlcohol, x.Picture)).ToList());
}