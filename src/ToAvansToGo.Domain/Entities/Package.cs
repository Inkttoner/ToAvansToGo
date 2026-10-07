using System.Runtime.InteropServices.JavaScript;
using ToAvansToGo.Domain.Enums;

namespace ToAvansToGo.Domain.Entities;

public class Package(int id, string name, MealType typeOfMeal, List<Product> products,  DateTime pickUpTime, DateTime availableTill, double price, Student? reservedBy )
{
    public int  Id { get; set; } = id;
    public  string Name { get; set; } =  name;
    public MealType TypeOfMeal { get; set; } = typeOfMeal;
    public  List<Product> Products { get; set; } = products;
    public  Canteen? PickUpLocation { get; set; } 
    public  DateTime PickUpTime { get; set; } = pickUpTime;
    public  DateTime AvailableTill { get; set; } =  availableTill;
    public bool Is18Plus { get; set; } 
    public double Price { get; set; } = price;
    public Student? ReservedBy { get; set; } = reservedBy;
}