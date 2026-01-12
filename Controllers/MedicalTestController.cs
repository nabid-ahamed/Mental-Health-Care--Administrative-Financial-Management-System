using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MHC_AFMS.Data;
using MHC_AFMS.Models;

namespace MHC_AFMS.Controllers
{
    // 🔒 SECURITY: Allows both Admin AND Staff
    [Authorize(Roles = "Admin,Staff")]
    public class MedicalTestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicalTestController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: List all tests
        public async Task<IActionResult> Index()
        {
            return View(await _context.MedicalTests.ToListAsync());
        }

        // GET: Create Page
        public IActionResult Create()
        {
            return View();
        }

        // POST: Add new test
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicalTest medicalTest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicalTest);
                await _context.SaveChangesAsync();
                TempData["Success"] = "New medical test added to catalog!";
                return RedirectToAction(nameof(Index));
            }
            return View(medicalTest);
        }

        // GET: Edit Page
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var test = await _context.MedicalTests.FindAsync(id);
            if (test == null) return NotFound();
            return View(test);
        }

        // POST: Save Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MedicalTest medicalTest)
        {
            if (id != medicalTest.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(medicalTest);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Test details updated!";
                return RedirectToAction(nameof(Index));
            }
            return View(medicalTest);
        }
    }
}