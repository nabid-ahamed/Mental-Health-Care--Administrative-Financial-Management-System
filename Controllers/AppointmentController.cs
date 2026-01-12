using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MHC_AFMS.Data;
using MHC_AFMS.Models;
using MHC_AFMS.Helpers;

namespace MHC_AFMS.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppointmentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ==========================================
        // 1. PATIENT: SEARCH & BOOKING
        // ==========================================

        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Index(string searchString)
        {
            var doctors = await _userManager.GetUsersInRoleAsync("Doctor");

            if (!string.IsNullOrEmpty(searchString))
            {
                doctors = doctors.Where(d => d.FullName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                                          || d.Specialization.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(doctors);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet]
        public async Task<IActionResult> Book(string doctorId, DateTime? date)
        {
            if (string.IsNullOrEmpty(doctorId)) return RedirectToAction("Index");

            DateTime selectedDate = date ?? DateTime.Today;

            var doctor = await _userManager.FindByIdAsync(doctorId);
            if (doctor == null) return NotFound();

            string dayName = selectedDate.DayOfWeek.ToString();
            var schedule = await _context.DoctorSchedules
                                         .FirstOrDefaultAsync(s => s.DoctorId == doctorId && s.DayOfWeek == dayName);

            ViewBag.Doctor = doctor;
            ViewBag.SelectedDate = selectedDate;
            ViewBag.HasSchedule = (schedule != null);

            var availableSlots = new List<AppointmentSlot>();

            if (schedule != null)
            {
                var bookedTimes = await _context.Appointments
                                                .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Date == selectedDate.Date)
                                                .Select(a => a.StartTime)
                                                .ToListAsync();

                ViewBag.BookedTimes = bookedTimes;

                availableSlots = SlotGenerator.GenerateSlots(schedule.StartHour, schedule.EndHour, new List<TimeSpan>());
            }

            return View(availableSlots);
        }

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> ConfirmBooking(string doctorId, DateTime date, TimeSpan startTime, string description)
        {
            var user = await _userManager.GetUserAsync(User);
            var doctor = await _userManager.FindByIdAsync(doctorId);

            if (user == null || doctor == null) return RedirectToAction("Index");

            bool isTaken = await _context.Appointments
                .AnyAsync(a => a.DoctorId == doctorId && a.AppointmentDate == date && a.StartTime == startTime);

            if (isTaken)
            {
                TempData["Error"] = "Sorry! Someone just booked that slot a second ago.";
                return RedirectToAction("Book", new { doctorId, date });
            }

            var appointment = new Appointment
            {
                PatientId = user.Id,
                DoctorId = doctorId,
                AppointmentDate = date,
                StartTime = startTime,
                EndTime = startTime.Add(new TimeSpan(0, 15, 0)),
                ConsultationFee = doctor.ConsultationFee,
                Description = description,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Appointment requested! Waiting for approval.";
            return RedirectToAction("MyAppointments");
        }

        // ==========================================
        // 2. PATIENT: VIEW HISTORY & ACCEPT/REJECT TESTS
        // ==========================================
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> MyAppointments()
        {
            var user = await _userManager.GetUserAsync(User);
            var list = await _context.Appointments
                                     .Include(a => a.Doctor)
                                     .Include(a => a.AppointmentTests)
                                        .ThenInclude(at => at.MedicalTest)
                                     .Where(a => a.PatientId == user.Id)
                                     .OrderByDescending(a => a.AppointmentDate)
                                     .ToListAsync();
            return View(list);
        }

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> AcceptTest(int id)
        {
            var test = await _context.AppointmentTests.FindAsync(id);
            if (test != null && test.Status == "Suggested")
            {
                test.Status = "Accepted";
                await _context.SaveChangesAsync();
                TempData["Success"] = "Test Accepted! Please proceed to the lab.";
            }
            return RedirectToAction("MyAppointments");
        }

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> RejectTest(int id)
        {
            var test = await _context.AppointmentTests.FindAsync(id);
            if (test != null && test.Status == "Suggested")
            {
                test.Status = "Rejected";
                await _context.SaveChangesAsync();
                TempData["Success"] = "Test Rejected.";
            }
            return RedirectToAction("MyAppointments");
        }

        // ==========================================
        // 3. STAFF & ADMIN SECTION
        // ==========================================

        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> ManageAppointments()
        {
            var allAppointments = await _context.Appointments
                                                .Include(a => a.Patient)
                                                .Include(a => a.Doctor)
                                                .OrderByDescending(a => a.AppointmentDate)
                                                .ThenBy(a => a.StartTime)
                                                .ToListAsync();
            return View(allAppointments);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Approve(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.Status = "Approved";
                await _context.SaveChangesAsync();
                TempData["Success"] = "Appointment Approved!";
            }
            return RedirectToAction("ManageAppointments");
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Cancel(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.Status = "Cancelled";
                await _context.SaveChangesAsync();
                TempData["Success"] = "Appointment Cancelled.";
            }
            return RedirectToAction("ManageAppointments");
        }

        // ==========================================
        // 4. DOCTOR SECTION
        // ==========================================

        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> DoctorAppointments()
        {
            var doctorId = _userManager.GetUserId(User);

            var myPatients = await _context.Appointments
                                           .Include(a => a.Patient)
                                           .Where(a => a.DoctorId == doctorId && a.Status == "Approved")
                                           .OrderBy(a => a.AppointmentDate)
                                           .ThenBy(a => a.StartTime)
                                           .ToListAsync();

            ViewBag.TestList = await _context.MedicalTests.ToListAsync();

            return View(myPatients);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> SubmitDiagnosis(int id, string diagnosis, string prescription, DateTime? followUpDate, List<int> recommendedTestIds)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment != null)
            {
                appointment.Diagnosis = diagnosis;
                appointment.Prescription = prescription;
                appointment.FollowUpDate = followUpDate;
                appointment.Status = "Completed";

                if (recommendedTestIds != null && recommendedTestIds.Any())
                {
                    foreach (var testId in recommendedTestIds)
                    {
                        var newTest = new AppointmentTest
                        {
                            AppointmentId = appointment.Id,
                            MedicalTestId = testId,
                            Status = "Suggested"
                        };
                        _context.AppointmentTests.Add(newTest);
                    }
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "Diagnosis & Treatment Plan saved successfully!";
            }
            return RedirectToAction("DoctorAppointments");
        }

        // ==========================================
        // 5. LAB & TEST MANAGEMENT (Merged Billing)
        // ==========================================

        // 🟢 THIS WAS MISSING IN YOUR CODE - CAUSING THE 404
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> ManageLabTests()
        {
            var pendingTests = await _context.AppointmentTests
                .Include(t => t.MedicalTest)
                .Include(t => t.Appointment)
                    .ThenInclude(a => a.Patient)
                .Where(t => t.Status == "Accepted")
                .ToListAsync();

            return View(pendingTests);
        }

        [HttpPost]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> MarkTestCompleted(int id)
        {
            // 1. Find the Test
            var testLink = await _context.AppointmentTests
                .Include(t => t.MedicalTest)
                .Include(t => t.Appointment)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (testLink == null) return NotFound();

            // 2. Mark Test as Completed
            testLink.Status = "Completed";

            // 3. MERGE BILLING LOGIC
            // Find the existing invoice for this appointment
            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.AppointmentId == testLink.AppointmentId);

            if (invoice != null && invoice.Status == "Unpaid")
            {
                // Add Test Cost to Existing Invoice
                invoice.Amount += testLink.MedicalTest.Price;

                // Update Appointment Total too for consistency
                testLink.Appointment.TotalAmount = invoice.Amount;

                TempData["Success"] = $"Test Completed. Added {testLink.MedicalTest.Price:C} to the invoice.";
            }
            else
            {
                // Edge case: If invoice is already paid or missing
                TempData["Warning"] = "Test marked Completed, but Invoice was not found or already paid.";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ManageLabTests");
        }
    }
}