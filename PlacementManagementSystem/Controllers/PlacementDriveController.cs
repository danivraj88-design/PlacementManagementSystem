using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlacementManagementSystem.Data;
using PlacementManagementSystem.Models;

namespace PlacementManagementSystem.Controllers
{
    [Authorize(Roles = "TPO")]
    public class PlacementDriveController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlacementDriveController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var drives = await _context.PlacementDrives
                .Include(d => d.JobOpenings)
                .OrderByDescending(d => d.DriveDate)
                .ToListAsync();

            return View(drives);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            PlacementDrive drive)
        {
            if (!ModelState.IsValid)
            {
                return View(drive);
            }

            drive.CreatedAt = DateTime.UtcNow;

            _context.PlacementDrives.Add(drive);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var drive = await _context.PlacementDrives
                .Include(d => d.JobOpenings)
                .ThenInclude(j => j.Company)
                .FirstOrDefaultAsync(d =>
                    d.PlacementDriveId == id);

            if (drive == null)
            {
                return NotFound();
            }

            return View(drive);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var drive = await _context.PlacementDrives
                .FindAsync(id);

            if (drive == null)
            {
                return NotFound();
            }

            _context.PlacementDrives.Remove(drive);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}