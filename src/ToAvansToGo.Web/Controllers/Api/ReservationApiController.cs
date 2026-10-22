using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToAvansToGo.Application;
using ToAvansToGo.Application.InterFaces;
using ToAvansToGo.Web.Dtos;

namespace ToAvansToGo.Web.Controllers.Api;

[ApiController]
[Route("api/packages/{packageId:int}/reservation")]
[Produces("application/json", "application/xml")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
public class ReservationsApiController(IReservationService reservationService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<ReservationResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Reserve(int packageId)
    {
        if (!int.TryParse(User.FindFirstValue("studentId"), out var studentId))
            return StatusCode(StatusCodes.Status403Forbidden, new ErrorResponse(403, "This account is not linked to a student"));

        try
        {
            await reservationService.ReservePackage(packageId, studentId);
            return Created($"/api/packages/{packageId}/reservation", new ReservationResponse(packageId, studentId));
        }
        catch (NotFoundException e)     { return NotFound(new ErrorResponse(404, e.Message)); }
        catch (ConflictException e)     { return Conflict(new ErrorResponse(409, e.Message)); }
        catch (BusinessRuleException e) { return UnprocessableEntity(new ErrorResponse(422, e.Message)); }
    }

    [AcceptVerbs("GET", "PUT", "DELETE")]
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult NotImplementedMethods() => ApiErrors.NotImplemented();
}