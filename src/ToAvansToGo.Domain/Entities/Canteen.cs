using ToAvansToGo.Domain.Enums;

namespace ToAvansToGo.Domain.Entities;

public class Canteen
{
    public int Id { get; set; }
    public City City { get; set; }
    public string Location { get; set; } = string.Empty;
    public bool HasHotMeals { get; set; }

    private Canteen() { } // voor EF Core

    public Canteen(int id, City city, string location, bool hasHotMeals)
    {
        Id = id;
        City = city;
        Location = location;
        HasHotMeals = hasHotMeals;
    }
}