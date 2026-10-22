using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToAvansToGo.Web.Dtos;
using ToAvansToGo.Application.InterFaces;
using ToAvansToGo.Application;
using ToAvansToGo.Infrastructure.Identity;

namespace ToAvansToGo.Web.Controllers.Api;




[ApiController]
[Route("api/packages/{packageId:int}/reservation")]
[Authorize(Roles = "Student")]  
public class ReservationApiController(IReservationService reservations, UserManager<ApplicationUser> userManager): ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Reserve(int packageId)
    {
        var user = await userManager.GetUserAsync(User);
        if (user?.StudentId is not { } studentId) return Forbid();

        try
        {
            await reservations.ReservePackage(packageId, studentId);
            return Created($"/api/packages/{packageId}/reservation", new { packageId, studentId });
        }
        catch (NotFoundException e)     { return NotFound(new { error = e.Message }); }
        catch (ConflictException e)     { return Conflict(new { error = e.Message }); }
        catch (BusinessRuleException e) { return UnprocessableEntity(new { error = e.Message }); }
    }
}