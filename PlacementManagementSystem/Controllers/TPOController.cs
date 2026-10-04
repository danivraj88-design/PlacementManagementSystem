using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlacementManagementSystem.Data;

namespace PlacementManagementSystem.Controllers
{
    [Authorize(Roles = "TPO")]
    public class TPOController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TPOController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalStudents =
                await _context.Students.CountAsync();

            ViewBag.TotalCompanies =
                await _context.Companies.CountAsync();

            ViewBag.TotalJobOpenings =
                await _context.JobOpenings.CountAsync();

            ViewBag.TotalApplications =
                await _context.PlacementApplications.CountAsync();

            return View();
        }
    }
}