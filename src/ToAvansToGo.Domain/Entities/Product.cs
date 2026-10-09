namespace ToAvansToGo.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; private set; } = string.Empty;
    public bool HasAlcohol { get; private set; }
    public string Picture { get; private set; } = string.Empty;

    public List<Package> Packages { get; set; } = new();

    private Product() { }

    public Product(int id, string name, bool hasAlcohol, string picture)
    {
        Id = id;
        Name = name;
        HasAlcohol = hasAlcohol;
        Picture = picture;
    }
}