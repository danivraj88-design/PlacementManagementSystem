using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlacementManagementSystem.Data;

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


        // ==========================================
        // COMPANY LIST
        // ==========================================
        public async Task<IActionResult> Index()
        {
            var companies = await _context.Companies
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(companies);
        }


        // ==========================================
        // COMPANY DETAILS
        // ==========================================
        public async Task<IActionResult> Details(int id)
        {
            var company = await _context.Companies
                .Include(c => c.User)
                .FirstOrDefaultAsync(c =>
                    c.CompanyId == id);

            if (company == null)
            {
                return NotFound(
                    "Company was not found.");
            }

            ViewBag.JobOpeningCount =
                await _context.JobOpenings
                    .CountAsync(j =>
                        j.CompanyId == company.CompanyId);

            ViewBag.ApplicationCount =
                await _context.PlacementApplications
                    .Where(a =>
                        a.JobOpening != null &&
                        a.JobOpening.CompanyId ==
                        company.CompanyId)
                    .CountAsync();

            return View(company);
        }


        // ==========================================
        // APPROVE COMPANY
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var company = await _context.Companies
                .FirstOrDefaultAsync(c =>
                    c.CompanyId == id);

            if (company == null)
            {
                return NotFound(
                    "Company was not found.");
            }

            company.IsApproved = true;
            company.ApprovedAt = DateTime.UtcNow;
            company.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TempData["Success"] =
                $"{company.CompanyName} has been approved successfully.";

            return RedirectToAction(nameof(Index));
        }


        // ==========================================
        // REJECT COMPANY
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var company = await _context.Companies
                .FirstOrDefaultAsync(c =>
                    c.CompanyId == id);

            if (company == null)
            {
                return NotFound(
                    "Company was not found.");
            }

            company.IsApproved = false;
            company.ApprovedAt = null;
            company.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            TempData["Success"] =
                $"{company.CompanyName} has been rejected.";

            return RedirectToAction(nameof(Index));
        }
    }
}