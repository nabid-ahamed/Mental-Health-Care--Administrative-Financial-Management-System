using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using MHC_AFMS.Models;

namespace MHC_AFMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // ==========================================
        //  LOGIN
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Admin")) return RedirectToAction("AdminDashboard");
                    if (roles.Contains("Doctor")) return RedirectToAction("DoctorDashboard");
                    if (roles.Contains("Staff")) return RedirectToAction("StaffDashboard");
                    if (roles.Contains("Patient")) return RedirectToAction("PatientDashboard");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Please enter email and password.";
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewBag.Error = "Invalid login attempt.";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Admin")) return RedirectToAction("AdminDashboard");
                if (roles.Contains("Doctor")) return RedirectToAction("DoctorDashboard");
                if (roles.Contains("Staff")) return RedirectToAction("StaffDashboard");
                if (roles.Contains("Patient")) return RedirectToAction("PatientDashboard");

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid login attempt.";
            return View();
        }

        // ==========================================
        //  PATIENT REGISTRATION
        // ==========================================

        [HttpGet]
        public IActionResult Register()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string fullName, string email, string password, string confirmPassword, int? age, string gender, string contactNumber, string address)
        {
            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                return View();
            }

            if (await _userManager.FindByEmailAsync(email) != null)
            {
                ViewBag.Error = "Email is already registered. Please Login.";
                return View();
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                Age = age,
                Gender = gender,
                ContactNumber = contactNumber,
                Address = address ?? "",
                Specialization = ""
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Patient");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("PatientDashboard");
            }

            ViewBag.Error = string.Join(", ", result.Errors.Select(e => e.Description));
            return View();
        }

        // ==========================================
        //  DASHBOARDS
        // ==========================================
        [Authorize(Roles = "Admin")] public IActionResult AdminDashboard() => View();
        [Authorize(Roles = "Doctor")] public IActionResult DoctorDashboard() => View();
        [Authorize(Roles = "Staff")] public IActionResult StaffDashboard() => View();
        [Authorize(Roles = "Patient")] public IActionResult PatientDashboard() => View();

        // ==========================================
        //  PROFILE SECTION
        // ==========================================
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            var model = new ProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                Username = user.UserName,
                ContactNumber = user.ContactNumber,
                Address = user.Address,
                Age = user.Age,
                Gender = user.Gender
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            user.FullName = model.FullName;
            user.ContactNumber = model.ContactNumber;
            user.Address = model.Address;
            user.Age = model.Age;
            user.Gender = model.Gender;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                TempData["Error"] = "Could not update profile.";
                return RedirectToAction("Profile");
            }

            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (string.IsNullOrEmpty(model.CurrentPassword))
                {
                    TempData["Error"] = "Current Password required.";
                    return RedirectToAction("Profile");
                }
                var passResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!passResult.Succeeded)
                {
                    TempData["Error"] = "Password Change Failed.";
                    return RedirectToAction("Profile");
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }

        // 🟢 FIX 2: LOGOUT REDIRECTS TO HOME PAGE
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public class ProfileViewModel
        {
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Username { get; set; }
            public string ContactNumber { get; set; }
            public string Address { get; set; }
            public int? Age { get; set; }
            public string Gender { get; set; }
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
        }
    }
}