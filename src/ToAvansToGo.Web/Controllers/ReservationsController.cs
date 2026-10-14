using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToAvansToGo.Application.InterFaces;
using ToAvansToGo.Domain.Enums;
using ToAvansToGo.Infrastructure.Identity;
using ToAvansToGo.Web.ViewModels;

namespace ToAvansToGo.Web.Controllers;


public class ReservationsController(IPackageService packageService, UserManager<ApplicationUser> userManager):Controller
{
    private async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        return await userManager.GetUserAsync(User);
    }
    public async Task<IActionResult> Index()
    {
        var user = await GetCurrentUserAsync();
        if (user?.StudentId is not { } studentId)
        {
            return Forbid();
        }
        var packages = await packageService.GetPackagesReservedByStudentAsync(studentId);

        foreach (var package in packages)
        {
            package.Products = await packageService.GetProductHistoryAsync(package.TypeOfMeal);
        }
        return View(packages);
    }
}