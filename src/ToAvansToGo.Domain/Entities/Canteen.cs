using ToAvansToGo.Domain.Enums;

namespace ToAvansToGo.Domain.Entities;

public class Canteen(int id,City city, string location, bool hasHotMeals)
{
    public int Id { get; set; } = id;
    public City City { get; set; } = city;
    public string Location { get; set; } = location;
    public bool HasHotMeals { get; set; }  = hasHotMeals;
}