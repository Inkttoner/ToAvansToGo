using Microsoft.AspNetCore.Mvc;
using ToAvansToGo.Application.InterFaces;

namespace ToAvansToGo.Web.Controllers;

public class CanteensController(ICanteenRepository canteenRepository) : Controller
{
    public async Task<IActionResult> Index()
    {
        var canteens = await canteenRepository.GetCanteensAsync();
        return View(canteens);
    }
}
