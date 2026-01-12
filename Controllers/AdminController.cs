using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MHC_AFMS.Models;
using MHC_AFMS.Data; // 🆕 Needed for ApplicationDbContext

namespace MHC_AFMS.Controllers
{
    [Authorize(Roles = "Admin")] // 🔒 ONLY Admin can access this
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context; // 🆕 Database Context

        // Updated Constructor to include 'context'
        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context; // 🆕 Assign it here
        }

        // 1. List All Users (Doctors, Staff, Admins)
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Specialization = user.Specialization,
                    ConsultationFee = user.ConsultationFee,
                    Role = roles.FirstOrDefault() ?? "None"
                });
            }

            return View(userList);
        }

        // 2. Hire User Page
        public IActionResult Create()
        {
            return View();
        }

        // 3. Hire User Logic (POST)
        [HttpPost]
        public async Task<IActionResult> Create(string fullName, string email, string password, string role, string specialization, decimal consultationFee)
        {
            // A. Check if email exists
            if (await _userManager.FindByEmailAsync(email) != null)
            {
                ViewBag.Error = "Email already exists!";
                return View();
            }

            // B. Create the User Object
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                EmailConfirmed = true,
                // Only save Doctor-specific fields if the role is Doctor
                Specialization = (role == "Doctor") ? specialization : "",
                ConsultationFee = (role == "Doctor") ? consultationFee : 0
            };

            // C. Save to Database with the Temporary Password
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // D. Assign Role
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
                await _userManager.AddToRoleAsync(user, role);

                TempData["Success"] = $"Successfully hired {fullName} as {role}.";
                return RedirectToAction("Index");
            }

            ViewBag.Error = "Error creating user: " + string.Join(", ", result.Errors.Select(e => e.Description));
            return View();
        }

        // 4. Fire/Delete User
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                // Prevent Admin from deleting themselves
                if (User.Identity.Name == user.UserName)
                {
                    TempData["Error"] = "You cannot delete your own account!";
                    return RedirectToAction("Index");
                }

                await _userManager.DeleteAsync(user);
                TempData["Success"] = "User deleted successfully.";
            }
            return RedirectToAction("Index");
        }

        // -----------------------------------------------------------
        // 🆕 NEW: SCHEDULING LOGIC
        // -----------------------------------------------------------

        // 5. Manage Schedule Page (GET)
        public async Task<IActionResult> ManageSchedule(string doctorId)
        {
            var doctor = await _userManager.FindByIdAsync(doctorId);
            if (doctor == null) return NotFound();

            ViewBag.DoctorName = doctor.FullName;
            ViewBag.DoctorId = doctor.Id;

            // Load existing schedule for this doctor
            var schedules = await _context.DoctorSchedules
                                          .Where(s => s.DoctorId == doctorId)
                                          .OrderBy(s => s.Id)
                                          .ToListAsync();
            return View(schedules);
        }

        // 6. Save Schedule Logic (POST)
        [HttpPost]
        public async Task<IActionResult> SaveSchedule(string doctorId, List<string> selectedDays, int startHour, int endHour)
        {
            // A. Validation: Clinic Hours (9 AM - 10 PM)
            if (startHour < 9 || endHour > 22 || startHour >= endHour)
            {
                TempData["Error"] = "Invalid hours! Clinic runs 9 AM - 10 PM, and Start must be before End.";
                return RedirectToAction("ManageSchedule", new { doctorId });
            }

            if (selectedDays == null || selectedDays.Count == 0)
            {
                TempData["Error"] = "Please select at least one day.";
                return RedirectToAction("ManageSchedule", new { doctorId });
            }

            // B. Clear old schedule for these days (Clean Slate Approach)
            var existing = _context.DoctorSchedules
                                   .Where(s => s.DoctorId == doctorId && selectedDays.Contains(s.DayOfWeek));
            _context.DoctorSchedules.RemoveRange(existing);

            // C. Add new fixed blocks
            foreach (var day in selectedDays)
            {
                _context.DoctorSchedules.Add(new DoctorSchedule
                {
                    DoctorId = doctorId,
                    DayOfWeek = day,
                    StartHour = startHour,
                    EndHour = endHour
                });
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Schedule updated successfully!";
            return RedirectToAction("ManageSchedule", new { doctorId });
        }

        // -----------------------------------------------------------
        // 🆕 EDIT DOCTOR / STAFF DETAILS
        // -----------------------------------------------------------

        // GET: Show Edit Form
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            // Reuse the UserViewModel we created earlier
            var model = new UserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Specialization = user.Specialization,
                ConsultationFee = user.ConsultationFee
            };

            // Pass the role to the view so we know if we should show doctor fields
            var roles = await _userManager.GetRolesAsync(user);
            model.Role = roles.FirstOrDefault();

            return View(model);
        }

        // POST: Save Changes
        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            // 1. Update Basic Info
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.UserName = model.Email; // Keep username synced with email

            // 2. Update Doctor Specifics (only if they are a doctor)
            if (await _userManager.IsInRoleAsync(user, "Doctor"))
            {
                user.Specialization = model.Specialization;
                user.ConsultationFee = model.ConsultationFee;
            }

            // 3. Save to DB
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["Success"] = "User details updated successfully!";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Error updating user.";
            return View(model);
        }
    }

    // Helper to display data in the list
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Specialization { get; set; }
        public decimal ConsultationFee { get; set; }
    }
}