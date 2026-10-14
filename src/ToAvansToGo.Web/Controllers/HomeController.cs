using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToAvansToGo.Application.InterFaces;
using ToAvansToGo.Domain.Enums;
using ToAvansToGo.Web.ViewModels;

namespace ToAvansToGo.Web.Controllers;

public class HomeController(IPackageService packageService) : Controller
{
    private const int PreviewCount = 3;

    
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var packages = await packageService.GetPackagesAsync(City.Breda, null );

        foreach (var package in packages)
        {
            package.Products = await packageService.GetProductHistoryAsync(package.TypeOfMeal);
        }
        
        var vm = new HomeViewModel
        {
            AvailableCount = packages.Count,
            Packages = packages.Take(PreviewCount).ToList()
        };
        return View(vm);
    }

    public IActionResult Error() => View();
}
