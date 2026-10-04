using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlacementManagementSystem.Data;
using PlacementManagementSystem.Models;

namespace PlacementManagementSystem.Areas.Identity.Pages.Account
{
    public class CompanyRegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public CompanyRegisterModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }

        // =====================================================
        // INPUT MODEL
        // =====================================================

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();


        public class InputModel
        {
            [Required(ErrorMessage = "Company name is required.")]
            [StringLength(150)]
            [Display(Name = "Company Name")]
            public string CompanyName { get; set; } = string.Empty;


            [Required(ErrorMessage = "Contact person is required.")]
            [StringLength(100)]
            [Display(Name = "Contact Person")]
            public string ContactPerson { get; set; } = string.Empty;


            [Required(ErrorMessage = "Official email is required.")]
            [EmailAddress(ErrorMessage = "Enter a valid email address.")]
            [StringLength(150)]
            [Display(Name = "Official Email")]
            public string Email { get; set; } = string.Empty;


            [Phone(ErrorMessage = "Enter a valid phone number.")]
            [StringLength(20)]
            [Display(Name = "Phone Number")]
            public string? PhoneNumber { get; set; }


            [Required(ErrorMessage = "Please select an industry.")]
            [StringLength(100)]
            public string Industry { get; set; } = string.Empty;


            [Url(ErrorMessage = "Enter a valid website URL.")]
            [StringLength(300)]
            public string? Website { get; set; }


            [StringLength(500)]
            public string? Address { get; set; }


            [StringLength(1000)]
            public string? Description { get; set; }


            [Required(ErrorMessage = "Password is required.")]
            [StringLength(
                100,
                MinimumLength = 6,
                ErrorMessage = "Password must be at least 6 characters."
            )]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;


            [Required(ErrorMessage = "Please confirm your password.")]
            [DataType(DataType.Password)]
            [Compare(
                "Password",
                ErrorMessage = "Password and confirmation password do not match."
            )]
            [Display(Name = "Confirm Password")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }


        // =====================================================
        // GET
        // =====================================================

        public void OnGet()
        {
        }


        // =====================================================
        // POST
        // =====================================================

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            // -------------------------------------------------
            // CHECK WHETHER EMAIL ALREADY EXISTS
            // -------------------------------------------------

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


            // -------------------------------------------------
            // CREATE COMPANY ROLE IF IT DOES NOT EXIST
            // -------------------------------------------------

            if (!await _roleManager.RoleExistsAsync("Company"))
            {
                var roleResult =
                    await _roleManager.CreateAsync(
                        new IdentityRole("Company")
                    );

                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError(
                            string.Empty,
                            error.Description
                        );
                    }

                    return Page();
                }
            }


            // -------------------------------------------------
            // CREATE IDENTITY USER
            // -------------------------------------------------

            var user = new ApplicationUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                FullName = Input.ContactPerson
            };


            var result =
                await _userManager.CreateAsync(
                    user,
                    Input.Password
                );


            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description
                    );
                }

                return Page();
            }


            // -------------------------------------------------
            // ASSIGN COMPANY ROLE
            // -------------------------------------------------

            var roleAssignment =
                await _userManager.AddToRoleAsync(
                    user,
                    "Company"
                );


            if (!roleAssignment.Succeeded)
            {
                foreach (var error in roleAssignment.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description
                    );
                }

                return Page();
            }


            // -------------------------------------------------
            // CREATE COMPANY PROFILE
            // -------------------------------------------------

            var company = new Company
            {
                UserId = user.Id,

                CompanyName = Input.CompanyName,

                ContactPerson = Input.ContactPerson,

                OfficialEmail = Input.Email,

                PhoneNumber = Input.PhoneNumber,

                Industry = Input.Industry,

                Website = Input.Website,

                Address = Input.Address,

                Description = Input.Description,

                IsApproved = false,

                CreatedAt = DateTime.UtcNow
            };


            _context.Companies.Add(company);

            await _context.SaveChangesAsync();


            // -------------------------------------------------
            // LOGIN COMPANY
            // -------------------------------------------------

            await _signInManager.SignInAsync(
                user,
                isPersistent: false
            );


            // -------------------------------------------------
            // GO TO COMPANY DASHBOARD
            // -------------------------------------------------

            return RedirectToAction(
                "Index",
                "Company"
            );
        }
    }
}