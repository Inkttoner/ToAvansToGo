namespace ToAvansToGo.Domain.Entities;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int EmployeeNumber { get; set; }

    public int CanteenId { get; set; }
    public Canteen Location { get; set; } = null!;

    private Employee() { }

    public Employee(int id, string name, int employeeNumber, Canteen location)
    {
        Id = id;
        Name = name;
        EmployeeNumber = employeeNumber;
        Location = location;
        CanteenId = location.Id;
    }
}