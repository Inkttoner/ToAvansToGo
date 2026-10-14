using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToAvansToGo.Application.InterFaces;
using ToAvansToGo.Domain.Entities;
using ToAvansToGo.Domain.Enums;
using ToAvansToGo.Infrastructure.Identity;
using ToAvansToGo.Web.ViewModels;

namespace ToAvansToGo.Web.Controllers;

public class PackagesController(
    IPackageService packageService,
    IReservationService reservationService,
    IProductRepository productRepository,
    ICanteenRepository canteenRepository,
    UserManager<ApplicationUser> userManager) : Controller
{

    private async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        return await userManager.GetUserAsync(User);
    }
    // GET /Packages?city=Breda&mealType=Hot   (voor studenten: beschikbare pakketten)
    [Authorize]
    public async Task<IActionResult> Index(City city = City.Breda, MealType? mealType = null)
    {
        var packages = await packageService.GetPackagesAsync(city, mealType);

        foreach (var package in packages)
        {
            package.Products = await packageService.GetProductHistoryAsync(package.TypeOfMeal);
        }
        var vm = new PackageIndexViewModel
        {
            City = city,
            MealType = mealType,
            Packages = packages
        };
        return View(vm);
    }

   [Authorize(Roles = "Employee")]
public async Task<IActionResult> Manage()
{
    var user = await GetCurrentUserAsync();

    if (user?.EmployeeId is not {} employeeId)
    {
        return Forbid();
    }

    var canteen =
        await canteenRepository.GetCanteenByEmployeeIdAsync(employeeId);

    var packages =
        await packageService.GetPackagesForCanteenAsync(true, canteen.Id);

    ViewData["Canteen"] = canteen;

    return View(packages);
}

    // GET /Packages/Details/5
    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var package = await packageService.GetPackageByIdAsync(id);
        if (package is null) return NotFound();
        return View(package);
    }

    // POST /Packages/Reserve/5
    [Authorize(Roles = "Student")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Reserve(int id)
    {
        var user = await GetCurrentUserAsync();

        if (user?.StudentId is not { } studentId)
        {
            return Forbid();
        }  
        try
        {
            await reservationService.ReservePackage(id, studentId);
            TempData["Success"] = "The package has been reserved.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Details), new { id });
    }

    // GET /Packages/Create
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> Create()
    {
        var vm = new PackageFormViewModel
        {
            PickUpTime = DateTime.Today.AddDays(1).AddHours(16),
            AvailableTill = DateTime.Today.AddDays(1).AddHours(18)
        };
        await FillOptionsAsync(vm);
        return View(vm);
    }

    // POST /Packages/Create
    [Authorize(Roles = "Employee")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PackageFormViewModel vm)
    {
        ValidateForm(vm);
        if (!ModelState.IsValid)
        {
            await FillOptionsAsync(vm);
            return View(vm);
        }
        var user  = await GetCurrentUserAsync();
        var products = await LoadProductsAsync(vm.SelectedProductIds);
        var package = new Package(0, vm.Name, vm.TypeOfMeal, products, vm.PickUpTime, vm.AvailableTill, vm.Price, null);

        if (user?.EmployeeId is not { } employeeId)
        {
            return Forbid();
        }
        try
        {
            // De service zet de ophaallocatie (kantine van de medewerker) en Is18Plus
            await packageService.CreatePackageAsync(package, employeeId);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await FillOptionsAsync(vm);
            return View(vm);
        }

        TempData["Success"] = "The package has been created.";
        return RedirectToAction(nameof(Manage));
    }

    // GET /Packages/Edit/5
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> Edit(int id)
    {
        var package = await packageService.GetPackageByIdAsync(id);
        if (package is null) return NotFound();

        if (package.ReservedBy is not null)
        {
            TempData["Error"] = "A reserved package can no longer be edited.";
            return RedirectToAction(nameof(Details), new { id });
        }

        var vm = new PackageFormViewModel
        {
            Id = package.Id,
            Name = package.Name,
            TypeOfMeal = package.TypeOfMeal,
            PickUpTime = package.PickUpTime,
            AvailableTill = package.AvailableTill,
            Price = package.Price,
            SelectedProductIds = package.Products.Select(p => p.Id).ToList()
        };
        await FillOptionsAsync(vm);
        return View(vm);
    }

    // POST /Packages/Edit/5
    [Authorize(Roles = "Employee")]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PackageFormViewModel vm)
    {
        if (id != vm.Id) return BadRequest();

        var package = await packageService.GetPackageByIdAsync(id);
        if (package is null) return NotFound();

        ValidateForm(vm);
        if (!ModelState.IsValid)
        {
            await FillOptionsAsync(vm);
            return View(vm);
        }

        var products = await LoadProductsAsync(vm.SelectedProductIds);

        package.Name = vm.Name;
        package.TypeOfMeal = vm.TypeOfMeal;
        package.PickUpTime = vm.PickUpTime;
        package.AvailableTill = vm.AvailableTill;
        package.Price = vm.Price;
        package.Products.Clear();
        package.Products.AddRange(products);

        try
        {
            await packageService.UpdatePackageAsync(package);
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Details), new { id });
        }

        TempData["Success"] = "The package has been saved.";
        return RedirectToAction(nameof(Details), new { id });
    }

    // GET /Packages/Delete/5
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> Delete(int id)
    {
        var package = await packageService.GetPackageByIdAsync(id);
        if (package is null) return NotFound();
        return View(package);
    }

    // POST /Packages/Delete/5
    [Authorize(Roles = "Employee")]
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var package = await packageService.GetPackageByIdAsync(id);
        if (package is null) return NotFound();

        try
        {
            await packageService.DeletePackageAsync(package);
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Details), new { id });
        }

        TempData["Success"] = "The package has been deleted.";
        return RedirectToAction(nameof(Manage));
    }

    //  Hulpmethodes 

    private void ValidateForm(PackageFormViewModel vm)
    {
        if (vm.SelectedProductIds.Count == 0)
        {
            ModelState.AddModelError(nameof(vm.SelectedProductIds), "Select at least one product.");
        }

        if (vm.AvailableTill < vm.PickUpTime)
        {
            ModelState.AddModelError(nameof(vm.AvailableTill), "Available until cannot be before the pick-up time.");
        }
    }

    private async Task FillOptionsAsync(PackageFormViewModel vm)
    {
        vm.AllProducts = await productRepository.GetProductsAsync();
    }

    private async Task<List<Product>> LoadProductsAsync(IEnumerable<int> ids)
    {
        var wanted = ids.ToHashSet();
        var all = await productRepository.GetProductsAsync();
        return all.Where(p => wanted.Contains(p.Id)).ToList();
    }
}
