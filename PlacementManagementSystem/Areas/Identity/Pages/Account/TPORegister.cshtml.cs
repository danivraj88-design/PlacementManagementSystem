using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlacementManagementSystem.Models;

namespace PlacementManagementSystem.Areas.Identity.Pages.Account
{
    public class TPORegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TPORegisterModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
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
            [Display(Name = "Official Email")]
            public string Email { get; set; } = string.Empty;

            [Required]
            [StringLength(50)]
            [Display(Name = "Employee ID")]
            public string EmployeeId { get; set; } = string.Empty;

            [Required]
            [StringLength(100)]
            public string Department { get; set; } = string.Empty;

            [Required]
            [StringLength(
                100,
                MinimumLength = 6,
                ErrorMessage = "Password must be at least 6 characters."
            )]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Compare(
                "Password",
                ErrorMessage = "Password and confirmation password do not match."
            )]
            [Display(Name = "Confirm Password")]
            public string ConfirmPassword { get; set; } = string.Empty;
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

            var existingUser =
                await _userManager.FindByEmailAsync(Input.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError(
                    "Input.Email",
                    "An account with this email already exists."
                );

                return Page();
            }

            // Make sure TPO role exists
            if (!await _roleManager.RoleExistsAsync("TPO"))
            {
                var roleResult =
                    await _roleManager.CreateAsync(
                        new IdentityRole("TPO"));

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

            var user = new ApplicationUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                FullName = Input.FullName
            };

            var result =
                await _userManager.CreateAsync(
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

            var roleResult2 =
                await _userManager.AddToRoleAsync(
                    user,
                    "TPO");

            if (!roleResult2.Succeeded)
            {
                foreach (var error in roleResult2.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return Page();
            }

            await _signInManager.SignInAsync(
                user,
                isPersistent: false);

            return RedirectToAction(
                "Index",
                "TPO");
        }
    }
}