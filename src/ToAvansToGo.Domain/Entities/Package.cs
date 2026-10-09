using ToAvansToGo.Domain.Enums;

namespace ToAvansToGo.Domain.Entities;

public class Package
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public MealType TypeOfMeal { get; set; }
    public List<Product> Products { get; set; } = new();

    public int? PickUpLocationId { get; set; }
    public Canteen? PickUpLocation { get; set; }

    public DateTime PickUpTime { get; set; }
    public DateTime AvailableTill { get; set; }
    public bool Is18Plus { get; set; }
    public double Price { get; set; }

    public int? ReservedById { get; set; }
    public Student? ReservedBy { get; set; }

    private Package() { }

    public Package(int id, string name, MealType typeOfMeal, List<Product> products,
        DateTime pickUpTime, DateTime availableTill, double price, Student? reservedBy)
    {
        Id = id;
        Name = name;
        TypeOfMeal = typeOfMeal;
        Products = products;
        PickUpTime = pickUpTime;
        AvailableTill = availableTill;
        Price = price;
        ReservedBy = reservedBy;
        ReservedById = reservedBy?.Id;
    }
}