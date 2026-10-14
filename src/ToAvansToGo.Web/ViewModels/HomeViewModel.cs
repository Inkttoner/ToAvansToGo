using ToAvansToGo.Domain.Entities;

namespace ToAvansToGo.Web.ViewModels;

public class HomeViewModel
{
    public int AvailableCount { get; set; }
    public List<Package> Packages { get; set; } = new();
}
