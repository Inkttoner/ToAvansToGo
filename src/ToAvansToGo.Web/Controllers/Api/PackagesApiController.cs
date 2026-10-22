using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToAvansToGo.Application.InterFaces;
using ToAvansToGo.Domain.Enums;
using ToAvansToGo.Web.Dtos;

namespace ToAvansToGo.Web.Controllers.Api;

[ApiController]
[Route("api/packages")]
[Produces("application/json", "application/xml")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
public class PackagesApiController(IPackageService packageService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<PackageDto>>> GetAll(City city = City.Breda, MealType? mealType = null)
    {
        var packages = await packageService.GetPackagesAsync(city, mealType);
        return Ok(packages.Select(p => p.ToDto()).ToList());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PackageDto>> GetById(int id)
    {
        var package = await packageService.GetPackageByIdAsync(id);
        return package is null
            ? NotFound(new ErrorResponse(404, "Package not found"))
            : Ok(package.ToDto());
    }

    [AcceptVerbs("POST", "PUT", "DELETE")]
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult NotImplementedOnCollection() => ApiErrors.NotImplemented();

    [AcceptVerbs("POST", "PUT", "DELETE", Route = "{id:int}")]
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult NotImplementedOnElement(int id) => ApiErrors.NotImplemented();
}