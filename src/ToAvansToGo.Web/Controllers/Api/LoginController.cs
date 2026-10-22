using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToAvansToGo.Infrastructure.Identity;

namespace ToAvansToGo.Web.Controllers.Api;

public record LoginRequest(string Email, string Password);

[ApiController]
[Route("api/login")]
public class LoginApiController(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Unauthorized(new { error = "Invalid email or password" });

        var result = await signInManager.PasswordSignInAsync(
            user, request.Password, isPersistent: false, lockoutOnFailure: true);

        if (!result.Succeeded)
            return Unauthorized(new { error = "Invalid email or password" });

        return Ok(new { email = user.Email, roles = await userManager.GetRolesAsync(user) });
    }
}