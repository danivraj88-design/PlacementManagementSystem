using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlacementManagementSystem.Data;
using PlacementManagementSystem.Models;

namespace PlacementManagementSystem.Controllers
{
    [Authorize(Roles = "Company")]
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompanyController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ============================
        // COMPANY DASHBOARD
        // ============================

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage(
                    "/Account/Login",
                    new { area = "Identity" });
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
            {
                return NotFound("Company profile was not found.");
            }

            ViewBag.TotalJobs =
                await _context.JobOpenings
                    .CountAsync(j => j.CompanyId == company.CompanyId);

            ViewBag.TotalApplications =
                await _context.PlacementApplications
                    .Where(a =>
                        a.JobOpening != null &&
                        a.JobOpening.CompanyId == company.CompanyId)
                    .CountAsync();

            return View(company);
        }

        // ============================
        // COMPANY PROFILE
        // ============================

        public async Task<IActionResult> Profile()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage(
                    "/Account/Login",
                    new { area = "Identity" });
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
            {
                return NotFound("Company profile was not found.");
            }

            return View(company);
        }

        // ============================
        // COMPANY JOBS
        // ============================

        public async Task<IActionResult> Jobs()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage(
                    "/Account/Login",
                    new { area = "Identity" });
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
            {
                return NotFound("Company profile was not found.");
            }

            var jobs = await _context.JobOpenings
                .Include(j => j.PlacementDrive)
                .Where(j => j.CompanyId == company.CompanyId)
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();

            return View(jobs);
        }

        // ============================
        // CREATE JOB - GET
        // ============================

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage(
                    "/Account/Login",
                    new { area = "Identity" });
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
            {
                return NotFound("Company profile was not found.");
            }

            if (!company.IsApproved)
            {
                TempData["Error"] =
                    "Your company must be approved by the TPO before creating job openings.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.PlacementDrives = await _context.PlacementDrives
                .OrderByDescending(d => d.PlacementDriveId)
                .ToListAsync();

            return View();
        }

        // ============================
        // CREATE JOB - POST
        // ============================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobOpening job)
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage(
                    "/Account/Login",
                    new { area = "Identity" });
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
            {
                return NotFound("Company profile was not found.");
            }

            if (!company.IsApproved)
            {
                TempData["Error"] =
                    "Your company must be approved by the TPO before creating job openings.";

                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                ViewBag.PlacementDrives = await _context.PlacementDrives
                    .OrderByDescending(d => d.PlacementDriveId)
                    .ToListAsync();

                return View(job);
            }

            // IMPORTANT:
            // CompanyId comes from the logged-in company.
            // We do NOT trust a CompanyId sent from the form.
            job.CompanyId = company.CompanyId;

            job.CreatedAt = DateTime.UtcNow;
            job.IsActive = true;

            _context.JobOpenings.Add(job);

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Job opening created successfully.";

            return RedirectToAction(nameof(Jobs));
        }

        // ============================
        // COMPANY APPLICATIONS
        // ============================

        public async Task<IActionResult> Applications()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage(
                    "/Account/Login",
                    new { area = "Identity" });
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (company == null)
            {
                return NotFound("Company profile was not found.");
            }

            var applications =
                await _context.PlacementApplications
                    .Include(a => a.Student)
                    .Include(a => a.JobOpening)
                    .Where(a =>
                        a.JobOpening != null &&
                        a.JobOpening.CompanyId == company.CompanyId)
                    .OrderByDescending(a => a.AppliedAt)
                    .ToListAsync();

            return View(applications);
        }
    }
}