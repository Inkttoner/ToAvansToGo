namespace ToAvansToGo.Web.Dtos;

public record ProductDto(int Id, string Name, bool HasAlcohol, string? Picture);

public record PackageDto(
    int Id, string Name, DateTime PickUpTime, DateTime AvailableTill,
    double Price, bool Is18Plus, string TypeOfMeal, string? Canteen,
    List<ProductDto> Products);

public record ReserveRequest(int StudentId);