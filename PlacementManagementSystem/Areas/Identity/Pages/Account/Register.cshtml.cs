using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlacementManagementSystem.Data;
using PlacementManagementSystem.Models;

namespace PlacementManagementSystem.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required]
            [StringLength(100)]
            [Display(Name = "Full Name")]
            public string FullName { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [StringLength(
                100,
                ErrorMessage = "Password must be at least 6 characters.",
                MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Compare(
                "Password",
                ErrorMessage = "Password and confirmation password do not match.")]
            [Display(Name = "Confirm Password")]
            public string ConfirmPassword { get; set; } = string.Empty;

            [Required]
            [StringLength(30)]
            [Display(Name = "Roll Number")]
            public string RollNumber { get; set; } = string.Empty;

            [Required]
            [StringLength(100)]
            [Display(Name = "Department")]
            public string Department { get; set; } = string.Empty;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check if email already exists
            var existingUser = await _userManager.FindByEmailAsync(Input.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError(
                    "Input.Email",
                    "An account with this email already exists.");

                return Page();
            }

            // ============================================
            // CREATE IDENTITY USER
            // ============================================

            var user = new ApplicationUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                FullName = Input.FullName
            };

            var result = await _userManager.CreateAsync(
                user,
                Input.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return Page();
            }

            // ============================================
            // MAKE SURE STUDENT ROLE EXISTS
            // ============================================

            if (!await _roleManager.RoleExistsAsync("Student"))
            {
                var roleResult = await _roleManager.CreateAsync(
                    new IdentityRole("Student"));

                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError(
                            string.Empty,
                            error.Description);
                    }

                    return Page();
                }
            }

            // ============================================
            // ASSIGN STUDENT ROLE
            // ============================================

            var roleAssignment =
                await _userManager.AddToRoleAsync(
                    user,
                    "Student");

            if (!roleAssignment.Succeeded)
            {
                foreach (var error in roleAssignment.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return Page();
            }

            // ============================================
            // CREATE STUDENT PROFILE
            // ============================================

            var student = new Student
            {
                UserId = user.Id,
                FullName = Input.FullName,
                RollNumber = Input.RollNumber,
                Department = Input.Department,
                CreatedAt = DateTime.UtcNow,
                IsProfileVerified = false,
                IsProfileFrozen = false
            };

            _context.Students.Add(student);

            await _context.SaveChangesAsync();

            // ============================================
            // SIGN IN STUDENT
            // ============================================


            await _signInManager.SignInAsync(
                user,
                isPersistent: false);

            // Refresh authentication cookie
            await _signInManager.RefreshSignInAsync(user);
            // ============================================
            // REDIRECT TO STUDENT DASHBOARD
            // ============================================

            return RedirectToAction(
                "Index",
                "Student");
        }
    }
}