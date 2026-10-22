
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToAvansToGo.Application.InterFaces;
using ToAvansToGo.Domain.Enums;
using ToAvansToGo.Web.Dtos;

namespace ToAvansToGo.Web.Controllers.Api;

[ApiController]
[Route("api/packages")]
[Authorize(Roles = "Student")]
public class PackagesApiController(IPackageService packageService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<PackageDto>>> GetAll(City city = City.Breda, MealType? mealType = null)
    {
        var packages = await packageService.GetPackagesAsync(city, mealType);
        return Ok(packages.Select(p => p.ToDto()).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PackageDto>> GetById(int id)
    {
        var package = await packageService.GetPackageByIdAsync(id);
        return package is null ? NotFound(new { error = "Package not found" }) : Ok(package.ToDto());
    }
}