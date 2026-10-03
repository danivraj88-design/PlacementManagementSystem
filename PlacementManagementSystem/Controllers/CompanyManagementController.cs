using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlacementManagementSystem.Data;
using PlacementManagementSystem.Models;

namespace PlacementManagementSystem.Controllers
{
    [Authorize(Roles = "TPO")]
    public class CompanyManagementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompanyManagementController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var companies = await _context.Companies
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(companies);
        }

        public async Task<IActionResult> Details(int id)
        {
            var company = await _context.Companies
                .Include(c => c.JobOpenings)
                .FirstOrDefaultAsync(c =>
                    c.CompanyId == id);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var company = await _context.Companies
                .FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            company.IsApproved = true;
            company.ApprovedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var company = await _context.Companies
                .FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            company.IsApproved = false;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}