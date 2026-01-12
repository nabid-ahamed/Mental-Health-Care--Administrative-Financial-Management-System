using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MHC_AFMS.Data;
using MHC_AFMS.Models;
using MHC_AFMS.Models.ViewModels; // Ensure you created the ViewModel in Step 1
using Microsoft.AspNetCore.Identity;

namespace MHC_AFMS.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ==========================================
        // STAFF: Create Invoices (Initial Bill)
        // ==========================================
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> UnbilledAppointments()
        {
            var unbilled = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.Status == "Completed" && a.IsInvoiceGenerated == false)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View(unbilled);
        }

        [HttpPost]
        [Authorize(Roles = "Staff,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateInvoice(int appointmentId)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null) return NotFound();

            decimal finalAmount = appointment.ConsultationFee;
            string note = "Standard Fee";

            // Follow-up Discount Logic
            var twoMonthsAgo = appointment.AppointmentDate.AddMonths(-2);
            bool isFollowUp = await _context.Appointments
                .AnyAsync(a => a.PatientId == appointment.PatientId
                            && a.DoctorId == appointment.DoctorId
                            && a.Status == "Completed"
                            && a.Id != appointment.Id
                            && a.AppointmentDate < appointment.AppointmentDate
                            && a.AppointmentDate >= twoMonthsAgo);

            if (isFollowUp)
            {
                decimal discount = finalAmount * 0.40m;
                finalAmount = finalAmount - discount;
                note = "40% Follow-up Discount Applied";
            }

            var invoice = new Invoice
            {
                AppointmentId = appointmentId,
                PatientId = appointment.PatientId,
                Amount = finalAmount,
                Status = "Unpaid",
                DueDate = DateTime.Now.AddDays(30)
            };

            _context.Invoices.Add(invoice);

            appointment.IsInvoiceGenerated = true;
            appointment.TotalAmount = finalAmount;

            await _context.SaveChangesAsync();
            TempData["Success"] = $"Invoice Generated: {finalAmount:C} ({note})";
            return RedirectToAction("UnbilledAppointments");
        }

        // ==========================================
        // PATIENT: Pay Bills (NEW GATEWAY LOGIC)
        // ==========================================
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> MyBills()
        {
            var user = await _userManager.GetUserAsync(User);

            var bills = await _context.Invoices
                                      .Include(i => i.Appointment)
                                        .ThenInclude(a => a.AppointmentTests)
                                      .Where(i => i.PatientId == user.Id)
                                      .OrderByDescending(i => i.DueDate)
                                      .ToListAsync();
            return View(bills);
        }

        // 1. GET: Show the Payment Gateway Page
        [Authorize(Roles = "Patient")]
        [HttpGet]
        public async Task<IActionResult> Gateway(int invoiceId)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Appointment)
                .ThenInclude(a => a.AppointmentTests)
                .FirstOrDefaultAsync(i => i.Id == invoiceId);

            if (invoice == null || invoice.Status == "Paid")
            {
                return RedirectToAction("MyBills");
            }

            // Security Check: Block if tests are pending
            bool hasPendingTests = invoice.Appointment.AppointmentTests
                .Any(t => t.Status == "Suggested" || t.Status == "Accepted");

            if (hasPendingTests)
            {
                TempData["Error"] = "Cannot pay yet! Tests are pending.";
                return RedirectToAction("MyBills");
            }

            var model = new PaymentGatewayViewModel
            {
                InvoiceId = invoice.Id,
                Amount = invoice.Amount
            };

            return View(model);
        }

        // 2. POST: Process the Fake Payment
        [HttpPost]
        [Authorize(Roles = "Patient")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(PaymentGatewayViewModel model)
        {
            // Simulate network delay
            await Task.Delay(1500);

            var invoice = await _context.Invoices.FindAsync(model.InvoiceId);
            if (invoice != null)
            {
                invoice.Status = "Paid";
                await _context.SaveChangesAsync();

                // Redirect to the Success Page
                return RedirectToAction("PaymentSuccess", new { id = invoice.Id });
            }

            return RedirectToAction("MyBills");
        }

        // 3. GET: Success Receipt Page
        [Authorize(Roles = "Patient")]
        public IActionResult PaymentSuccess(int id)
        {
            // You can use your existing Success.cshtml, but passing the ID is helpful
            return View(id);
        }
    }
}