using Microsoft.AspNetCore.Mvc;
using ToAvansToGo.Domain.Entities;

namespace ToAvansToGo.Web.Dtos;

public record ProductDto(int Id, string Name, bool HasAlcohol, string? Picture);

public record PackageDto(
    int Id, string Name, DateTime PickUpTime, DateTime AvailableTill,
    double Price, bool Is18Plus, string TypeOfMeal, string? Canteen,
    List<ProductDto> Products);

public record LoginRequest(string Email, string Password);
public record LoginResponse(string AccessToken, DateTime ExpiresAt);
public record ReservationResponse(int PackageId, int StudentId);
public record ErrorResponse(int Status, string Message);

public static class PackageMapping
{
    public static PackageDto ToDto(this Package p) => new(
        p.Id, p.Name, p.PickUpTime, p.AvailableTill, p.Price, p.Is18Plus,
        p.TypeOfMeal.ToString(), p.PickUpLocation?.Location,
        p.Products.Select(x => new ProductDto(x.Id, x.Name, x.HasAlcohol, x.Picture)).ToList());
}

public static class ApiErrors
{
    public static ObjectResult NotImplemented() =>
        new(new ErrorResponse(501, "This method is not implemented for this resource"))
        {
            StatusCode = StatusCodes.Status501NotImplemented
        };
}