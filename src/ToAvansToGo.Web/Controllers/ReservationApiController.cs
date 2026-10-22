using Microsoft.AspNetCore.Mvc;
using ToAvansToGo.Web.Dtos;
using ToAvansToGo.Application.InterFaces;
using ToAvansToGo.Application;

namespace ToAvansToGo.Web.Controllers;




[ApiController]
[Route("api/packages/{packageId:int}/reservation")]
public class ReservationApiController(IReservationService reservations): ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Reserve(int packageId, [FromBody] ReserveRequest request)
    {
        try
        {
            await reservations.ReservePackage(packageId, request.StudentId);
            return Created($"/api/packages/{packageId}/reservation",
                new { packageId, studentId = request.StudentId });
        }
        catch (NotFoundException e)     { return NotFound(new { error = e.Message }); }
        catch (ConflictException e)     { return Conflict(new { error = e.Message }); }
        catch (BusinessRuleException e) { return UnprocessableEntity(new { error = e.Message }); }
    }
}