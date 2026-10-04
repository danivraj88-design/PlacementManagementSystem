using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlacementManagementSystem.Data;

namespace PlacementManagementSystem.Controllers
{
    [Authorize(Roles = "TPO")]
    public class EligibilityController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EligibilityController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(
            decimal minimumCGPA,
            int maximumBacklogs,
            string? department)
        {
            var students = _context.Students
                .AsQueryable();

            students = students.Where(s =>
                s.CurrentCGPA >= minimumCGPA &&
                s.ActiveBacklogs <= maximumBacklogs);

            if (!string.IsNullOrWhiteSpace(department))
            {
                students = students.Where(s =>
                    s.Department == department);
            }

            var result = await students
                .OrderByDescending(s => s.CurrentCGPA)
                .ToListAsync();

            ViewBag.MinimumCGPA = minimumCGPA;
            ViewBag.MaximumBacklogs = maximumBacklogs;
            ViewBag.Department = department;

            return View(result);
        }
    }
}