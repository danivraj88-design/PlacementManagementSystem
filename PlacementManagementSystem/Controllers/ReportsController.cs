using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlacementManagementSystem.Data;

namespace PlacementManagementSystem.Controllers
{
    [Authorize(Roles = "TPO")]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalStudents =
                await _context.Students.CountAsync();

            ViewBag.TotalCompanies =
                await _context.Companies.CountAsync();

            ViewBag.TotalJobs =
                await _context.JobOpenings.CountAsync();

            ViewBag.TotalApplications =
                await _context.PlacementApplications.CountAsync();

            ViewBag.SelectedStudents =
                await _context.PlacementApplications
                    .CountAsync(a =>
                        a.Status == "Selected");

            ViewBag.ShortlistedStudents =
                await _context.PlacementApplications
                    .CountAsync(a =>
                        a.Status == "Shortlisted");

            ViewBag.RejectedApplications =
                await _context.PlacementApplications
                    .CountAsync(a =>
                        a.Status == "Rejected");

            return View();
        }
    }
}