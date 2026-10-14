using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ToAvansToGo.Infrastructure.Identity;

public class ToAvansToGoIdentityDbContext(
    DbContextOptions<ToAvansToGoIdentityDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
}