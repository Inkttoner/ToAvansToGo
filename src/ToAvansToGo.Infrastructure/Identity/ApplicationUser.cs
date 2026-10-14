using Microsoft.AspNetCore.Identity;

namespace ToAvansToGo.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public int? StudentId { get; set; }

    public int? EmployeeId { get; set; }
}