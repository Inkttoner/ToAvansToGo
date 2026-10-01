namespace ToAvansToGo.Domain.Entities;

public class Product(int id, string name, bool hasAlcohol, string picture)
{
    public int  Id { get; set; } = id;
    public string Name { get; private set; } = name;
    public bool HasAlcohol { get; private set; } = hasAlcohol;
    public string Picture { get; private set; } = picture;
}