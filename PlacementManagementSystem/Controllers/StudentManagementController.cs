using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlacementManagementSystem.Data;

namespace PlacementManagementSystem.Controllers
{
    [Authorize(Roles = "TPO")]
    public class StudentManagementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentManagementController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string? search)
        {
            var students = _context.Students
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                students = students.Where(s =>
                    s.FullName.Contains(search) ||
                    s.RollNumber.Contains(search) ||
                    s.Department.Contains(search));
            }

            ViewBag.Search = search;

            return View(await students
                .OrderBy(s => s.FullName)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s =>
                    s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }
    }
}