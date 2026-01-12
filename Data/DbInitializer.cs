using Microsoft.AspNetCore.Identity;
using MHC_AFMS.Models;

namespace MHC_AFMS.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "Doctor", "Staff", "Patient" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 1. Admin
            if (await userManager.FindByEmailAsync("admin@hospital.com") == null)
            {
                var admin = new ApplicationUser { UserName = "admin@hospital.com", Email = "admin@hospital.com", FullName = "Super Admin", EmailConfirmed = true };
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // 2. Doctor
            if (await userManager.FindByEmailAsync("doctor@hospital.com") == null)
            {
                var doctor = new ApplicationUser { UserName = "doctor@hospital.com", Email = "doctor@hospital.com", FullName = "Dr. Strange", EmailConfirmed = true };
                await userManager.CreateAsync(doctor, "Doctor123!");
                await userManager.AddToRoleAsync(doctor, "Doctor");
            }

            // 3. Staff
            if (await userManager.FindByEmailAsync("staff@hospital.com") == null)
            {
                var staff = new ApplicationUser { UserName = "staff@hospital.com", Email = "staff@hospital.com", FullName = "Nurse Joy", EmailConfirmed = true };
                await userManager.CreateAsync(staff, "Staff123!");
                await userManager.AddToRoleAsync(staff, "Staff");
            }

            // 4. Patient
            if (await userManager.FindByEmailAsync("patient@hospital.com") == null)
            {
                var patient = new ApplicationUser { UserName = "patient@hospital.com", Email = "patient@hospital.com", FullName = "John Doe", EmailConfirmed = true };
                await userManager.CreateAsync(patient, "Patient123!");
                await userManager.AddToRoleAsync(patient, "Patient");
            }
        }
    }
}