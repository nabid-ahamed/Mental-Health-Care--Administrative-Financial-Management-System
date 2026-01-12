using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MHC_AFMS.Data;
using MHC_AFMS.Models;
using MHC_AFMS.Models.ViewModels;

namespace MHC_AFMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FinancialController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FinancialController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard(DateTime? startDate, DateTime? endDate)
        {
            // Default to THIS MONTH if no date selected
            DateTime start = startDate ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime end = endDate ?? DateTime.Now.Date.AddHours(23).AddMinutes(59);

            var viewModel = new AdminFinancialViewModel
            {
                StartDate = start,
                EndDate = end
            };

            // ==========================================
            // 1. & 9. OVERALL REVENUE & DAILY SUMMARY
            // ==========================================
            var allInvoices = await _context.Invoices
                .Include(i => i.Appointment)
                .ToListAsync();

            // Today's Stats
            viewModel.RevenueToday = allInvoices
                .Where(i => i.Appointment.AppointmentDate.Date == DateTime.Today)
                .Sum(i => i.Amount);

            viewModel.AppointmentsToday = await _context.Appointments
                .CountAsync(a => a.AppointmentDate.Date == DateTime.Today);

            viewModel.TestsToday = await _context.AppointmentTests
                .CountAsync(t => t.Status == "Completed" && t.Appointment.AppointmentDate.Date == DateTime.Today);

            // Revenue This Month
            viewModel.RevenueThisMonth = allInvoices
                .Where(i => i.Appointment.AppointmentDate.Month == DateTime.Now.Month)
                .Sum(i => i.Amount);

            // Custom Range Revenue
            var rangeInvoices = allInvoices
                .Where(i => i.Appointment.AppointmentDate >= start && i.Appointment.AppointmentDate <= end)
                .ToList();

            viewModel.RevenueTotal = rangeInvoices.Sum(i => i.Amount);

            // ==========================================
            // 2, 3, 5. DETAILED ANALYTICS (The Fix is Here)
            // ==========================================

            var appointmentsInRange = await _context.Appointments
                .Include(a => a.Doctor) // 🟢 ADDED THIS: Loads Doctor Data
                .Include(a => a.AppointmentTests).ThenInclude(at => at.MedicalTest)
                .Where(a => a.AppointmentDate >= start && a.AppointmentDate <= end && a.Status == "Completed")
                .ToListAsync();

            // 2. REVENUE BY CATEGORY
            viewModel.TotalConsultationFees = appointmentsInRange.Sum(a => a.ConsultationFee);

            viewModel.TotalTestFees = appointmentsInRange
                .SelectMany(a => a.AppointmentTests)
                .Where(t => t.Status == "Completed")
                .Sum(t => t.MedicalTest.Price);

            // 3. REVENUE BY DOCTOR (Fixed Crash)
            viewModel.DoctorStats = appointmentsInRange
                .GroupBy(a => a.Doctor)
                .Select(g => new DoctorRevenueStats
                {
                    // 🟢 ADDED SAFETY CHECK (?.FullName ?? "Unknown")
                    DoctorName = g.Key?.FullName ?? "Unknown Doctor",
                    AppointmentCount = g.Count(),
                    TotalEarnings = g.Sum(a => a.TotalAmount)
                })
                .OrderByDescending(x => x.TotalEarnings)
                .ToList();

            // 5. REVENUE BY TEST TYPE
            var testsInRange = await _context.AppointmentTests
                .Include(t => t.MedicalTest)
                .Include(t => t.Appointment)
                .Where(t => t.Status == "Completed" && t.Appointment.AppointmentDate >= start && t.Appointment.AppointmentDate <= end)
                .ToListAsync();

            viewModel.TestStats = testsInRange
                .GroupBy(t => t.MedicalTest)
                .Select(g => new TestRevenueStats
                {
                    TestName = g.Key.TestName,
                    Count = g.Count(),
                    TotalIncome = g.Sum(t => t.MedicalTest.Price)
                })
                .OrderByDescending(x => x.TotalIncome)
                .ToList();

            // 6. PAYMENT STATUS TRACKING
            viewModel.PaidPayments = rangeInvoices.Where(i => i.Status == "Paid").Sum(i => i.Amount);
            viewModel.PendingPayments = rangeInvoices.Where(i => i.Status == "Unpaid").Sum(i => i.Amount);

            return View(viewModel);
        }
    }
}