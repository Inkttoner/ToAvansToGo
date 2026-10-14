using Microsoft.EntityFrameworkCore;
using ToAvansToGo.Application.InterFaces;
using ToAvansToGo.Infrastructure.Data;
using ToAvansToGo.Infrastructure.Repositories;
using ToAvansToGo.Application.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using ToAvansToGo.Web;

var builder = WebApplication.CreateBuilder(args);

// 1. MVC
builder.Services.AddControllersWithViews();

// 2. Database (EF Core)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource)));

// 3. Repositories (interface -> implementatie)
builder.Services.AddScoped<ICanteenRepository, CanteenRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IPackageRepository, PackageRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// 4. Hier komen later je services uit de applicatielaag, bijvoorbeeld:
builder.Services.AddScoped<IPackageService, PackageService>();
builder.Services.AddScoped<IReservationService, ReservationService>();



var app = builder.Build();
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(culture: "en-GB", uiCulture: "en"),
    SupportedCultures = new[] { new CultureInfo("en-GB") },
    SupportedUICultures = new[] { new CultureInfo("en"), new CultureInfo("nl") }
});
// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();