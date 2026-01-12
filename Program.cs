using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MHC_AFMS.Data;
using MHC_AFMS.Models;
using System.Globalization; // 1. Added namespace for Globalization

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------
// 1. Configure Database (MySQL with Pomelo Driver)
// --------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// --------------------------------------------------
// 2. Add Identity (User & Role Management)
// --------------------------------------------------
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// --------------------------------------------------
// 3. Add Services (MVC + Razor Pages)
// --------------------------------------------------
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ==================================================
// 🟢 NEW: GLOBAL CURRENCY CONFIGURATION (৳ BDT)
// ==================================================
var cultureInfo = new CultureInfo("en-US"); // Start with US English (for 1,234.56 format)
cultureInfo.NumberFormat.CurrencySymbol = "\u09F3"; // Replace $ with ৳

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;


// --------------------------------------------------
// 4. Build App
// --------------------------------------------------
var app = builder.Build();

// --------------------------------------------------
// 5. Configure Pipeline
// --------------------------------------------------

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Session must be before Auth
app.UseSession();

// Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

// --------------------------------------------------
// 6. Routes
// --------------------------------------------------

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}" 
);

// This is required for Identity's default UI
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Call our new seeder
        await DbInitializer.Initialize(services, userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();