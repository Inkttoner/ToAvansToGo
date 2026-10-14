
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToAvansToGo.Infrastructure.Data;

namespace ToAvansToGo.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        IServiceProvider services,
        IConfiguration configuration)
    {
        var dbContext =
            services.GetRequiredService<ApplicationDbContext>();

        var userManager =
            services.GetRequiredService<UserManager<ApplicationUser>>();

        var roleManager =
            services.GetRequiredService<RoleManager<IdentityRole>>();

        var password = configuration["IdentitySeed:Password"];

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "Configureer IdentitySeed:Password via User Secrets.");
        }

        // 1. Rollen aanmaken
        foreach (var roleName in new[] { "Student", "Employee" })
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(
                    new IdentityRole(roleName));

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join("; ",
                            result.Errors.Select(e => e.Description)));
                }
            }
        }

        // 2. Bestaande personen ophalen
        var students = await dbContext.Students
            .OrderBy(s => s.Id)
            .Take(2)
            .ToListAsync();

        var employees = await dbContext.Employees
            .OrderBy(e => e.Id)
            .Take(2)
            .ToListAsync();

        if (students.Count < 2 || employees.Count < 2)
        {
            throw new InvalidOperationException(
                "De applicatiedatabase moet minimaal twee studenten " +
                "en twee medewerkers bevatten voordat Identity wordt geseed.");
        }

        // 3. Accounts voor studenten
        foreach (var student in students)
        {
            await EnsureUserAsync(
                userManager,
                student.Email,
                student.Id,
                null,
                "Student",
                password);
        }

        // 4. Accounts voor medewerkers
        foreach (var employee in employees)
        {
            var email = employee.Name
                .ToLowerInvariant()
                .Replace(" ", ".") + "@employee.avans.nl";

            await EnsureUserAsync(
                userManager,
                email,
                null,
                employee.Id,
                "Employee",
                password);
        }
    }

    private static async Task EnsureUserAsync(
        UserManager<ApplicationUser> userManager,
        string email,
        int? studentId,
        int? employeeId,
        string role,
        string password)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                StudentId = studentId,
                EmployeeId = employeeId
            };

            var createResult =
                await userManager.CreateAsync(user, password);

            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Account {email} kon niet worden aangemaakt: " +
                    string.Join("; ",
                        createResult.Errors.Select(e => e.Description)));
            }
        }
        else
        {
            user.StudentId = studentId;
            user.EmployeeId = employeeId;

            var updateResult = await userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Account {email} kon niet worden bijgewerkt: " +
                    string.Join("; ",
                        updateResult.Errors.Select(e => e.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            var roleResult =
                await userManager.AddToRoleAsync(user, role);

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Rol {role} kon niet worden toegewezen aan {email}: " +
                    string.Join("; ",
                        roleResult.Errors.Select(e => e.Description)));
            }
        }
    }
}
