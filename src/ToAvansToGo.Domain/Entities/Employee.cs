namespace ToAvansToGo.Domain.Entities;

public class Employee(int id, string name, int employeeNumber, Canteen location)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public int EmployeeNumber { get; set; } = employeeNumber;
    public  Canteen Location { get; set; } =  location;
}