using ToAvansToGo.Domain.Entities;
using ToAvansToGo.Domain.Enums;

namespace ToAvansToGo.Web.ViewModels;

public class PackageIndexViewModel
{
    public City City { get; set; }
    public MealType? MealType { get; set; }
    public List<Package> Packages { get; set; } = new();
}
