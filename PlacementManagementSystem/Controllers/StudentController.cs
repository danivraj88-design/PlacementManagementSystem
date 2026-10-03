using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlacementManagementSystem.Data;
using PlacementManagementSystem.Models;

namespace PlacementManagementSystem.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ============================================
        // STUDENT DASHBOARD
        // ============================================

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login", new
                {
                    area = "Identity"
                });
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null)
            {
                return NotFound("Student profile was not found.");
            }

            return View(student);
        }

        // ============================================
        // STUDENT PROFILE
        // ============================================

        public async Task<IActionResult> Profile()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login", new
                {
                    area = "Identity"
                });
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null)
            {
                return NotFound("Student profile was not found.");
            }

            return View(student);
        }

        // ============================================
        // EDIT PROFILE
        // ============================================

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login", new
                {
                    area = "Identity"
                });
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null)
            {
                return NotFound("Student profile was not found.");
            }

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(Student student)
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login", new
                {
                    area = "Identity"
                });
            }

            var existingStudent = await _context.Students
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (existingStudent == null)
            {
                return NotFound("Student profile was not found.");
            }
            // UserId is managed by ASP.NET Identity.
            // It is not submitted from the edit profile form.
            ModelState.Remove(nameof(Student.UserId));
            ModelState.Remove(nameof(Student.User));

            if (!ModelState.IsValid)
            {
                return View(student);
            }

            existingStudent.FullName = student.FullName;
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                user.FullName = student.FullName;
                await _userManager.UpdateAsync(user);
            }
            existingStudent.RollNumber = student.RollNumber;
            existingStudent.Department = student.Department;
            existingStudent.PhoneNumber = student.PhoneNumber;
            existingStudent.DateOfBirth = student.DateOfBirth;
            existingStudent.Gender = student.Gender;

            existingStudent.PresentAddress = student.PresentAddress;
            existingStudent.PermanentAddress = student.PermanentAddress;

            existingStudent.EmergencyContactName =
                student.EmergencyContactName;

            existingStudent.EmergencyContactNumber =
                student.EmergencyContactNumber;

            existingStudent.TenthPercentage =
                student.TenthPercentage;

            existingStudent.TwelfthPercentage =
                student.TwelfthPercentage;

            existingStudent.DiplomaPercentage =
                student.DiplomaPercentage;

            existingStudent.CurrentCGPA =
                student.CurrentCGPA;

            existingStudent.ActiveBacklogs =
                student.ActiveBacklogs;

            existingStudent.ClearedBacklogs =
                student.ClearedBacklogs;

            existingStudent.PassingYear =
                student.PassingYear;

            existingStudent.Specialization =
                student.Specialization;

            existingStudent.Skills =
                student.Skills;

            existingStudent.GitHubUrl =
                student.GitHubUrl;

            existingStudent.LinkedInUrl =
                student.LinkedInUrl;

            existingStudent.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Profile));
        }
    }
}