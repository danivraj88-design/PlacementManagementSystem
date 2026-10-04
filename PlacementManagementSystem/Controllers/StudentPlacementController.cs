using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlacementManagementSystem.Data;
using PlacementManagementSystem.Models;

namespace PlacementManagementSystem.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentPlacementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentPlacementController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ============================================
        // STUDENT - VIEW PLACEMENT DRIVES
        // ============================================
        public async Task<IActionResult> Drives()
        {
            // Get all active placement drives
            var drives = await _context.PlacementDrives
                .Where(d => d.IsActive)
                .OrderBy(d => d.DriveDate)
                .ToListAsync();

            // Load jobs separately for every placement drive
            foreach (var drive in drives)
            {
                drive.JobOpenings = await _context.JobOpenings
                    .Include(j => j.Company)
                    .Where(j =>
                        j.PlacementDriveId == drive.PlacementDriveId &&
                        j.IsActive)
                    .ToListAsync();
            }

            return View(drives);
        }


        // ============================================
        // STUDENT - APPLY FOR JOB
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(int id)
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage(
                    "/Account/Login",
                    new { area = "Identity" });
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null)
            {
                return NotFound("Student profile was not found.");
            }

            var job = await _context.JobOpenings
                .Include(j => j.Company)
                .Include(j => j.PlacementDrive)
                .FirstOrDefaultAsync(
                    j => j.JobOpeningId == id &&
                         j.IsActive);

            if (job == null)
            {
                return NotFound("Job opening was not found.");
            }

            // Company approval check
            if (job.Company == null || !job.Company.IsApproved)
            {
                TempData["Error"] =
                    "You cannot apply because this company is not approved.";

                return RedirectToAction(nameof(Drives));
            }

            // Deadline check
            if (job.ApplicationDeadline.HasValue &&
                job.ApplicationDeadline.Value.Date < DateTime.Today)
            {
                TempData["Error"] =
                    "The application deadline has passed.";

                return RedirectToAction(nameof(Drives));
            }

            // Already applied check
            var alreadyApplied =
                await _context.PlacementApplications
                    .AnyAsync(a =>
                        a.StudentId == student.StudentId &&
                        a.JobOpeningId == job.JobOpeningId);

            if (alreadyApplied)
            {
                TempData["Error"] =
                    "You have already applied for this job.";

                return RedirectToAction(nameof(Drives));
            }

            // CGPA check
            if (!student.CurrentCGPA.HasValue ||
                student.CurrentCGPA.Value < job.MinimumCGPA)
            {
                TempData["Error"] =
                    $"You are not eligible. Minimum CGPA required is {job.MinimumCGPA:0.00}.";

                return RedirectToAction(nameof(Drives));
            }

            // Backlog check
            if (student.ActiveBacklogs > job.MaximumBacklogs)
            {
                TempData["Error"] =
                    $"You are not eligible. Maximum allowed backlogs are {job.MaximumBacklogs}.";

                return RedirectToAction(nameof(Drives));
            }

            // Department check
            if (!string.IsNullOrWhiteSpace(job.EligibleDepartments))
            {
                var departments = job.EligibleDepartments
                    .Split(
                        ',',
                        StringSplitOptions.RemoveEmptyEntries)
                    .Select(d => d.Trim())
                    .ToList();

                var departmentAllowed = departments.Any(d =>
                    string.Equals(
                        d,
                        student.Department,
                        StringComparison.OrdinalIgnoreCase));

                if (!departmentAllowed)
                {
                    TempData["Error"] =
                        "Your department is not eligible for this job.";

                    return RedirectToAction(nameof(Drives));
                }
            }

            var application = new PlacementApplication
            {
                StudentId = student.StudentId,
                JobOpeningId = job.JobOpeningId,
                Status = "Applied",
                AppliedAt = DateTime.UtcNow
            };

            _context.PlacementApplications.Add(application);

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Application submitted successfully.";

            return RedirectToAction(nameof(Applications));
        }


        // ============================================
        // STUDENT - MY APPLICATIONS
        // ============================================
        public async Task<IActionResult> Applications()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage(
                    "/Account/Login",
                    new { area = "Identity" });
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null)
            {
                return NotFound(
                    "Student profile was not found.");
            }

            var applications =
                await _context.PlacementApplications

                    .Include(a => a.JobOpening)
                        .ThenInclude(j => j.Company)

                    .Include(a => a.JobOpening)
                        .ThenInclude(j => j.PlacementDrive)

                    .Where(a =>
                        a.StudentId == student.StudentId)

                    .OrderByDescending(a => a.AppliedAt)

                    .ToListAsync();

            return View(applications);
        }
    }
}