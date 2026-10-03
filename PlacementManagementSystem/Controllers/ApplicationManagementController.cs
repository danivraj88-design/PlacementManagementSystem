using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlacementManagementSystem.Data;

namespace PlacementManagementSystem.Controllers
{
    [Authorize(Roles = "TPO")]
    public class ApplicationManagementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationManagementController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applications =
                await _context.PlacementApplications
                .Include(a => a.Student)
                .Include(a => a.JobOpening)
                    .ThenInclude(j => j.Company)
                .OrderByDescending(a => a.AppliedAt)
                .ToListAsync();

            return View(applications);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(
            int id,
            string status)
        {
            var application =
                await _context.PlacementApplications
                .FindAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            application.Status = status;
            application.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}