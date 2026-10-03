using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PlacementManagementSystem.Models;

namespace PlacementManagementSystem.Areas.Identity.Pages.Account;

public class LoginModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ILogger<LoginModel> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public IList<AuthenticationScheme>? ExternalLogins { get; set; }

    public string? ReturnUrl { get; set; }

    public string SelectedRole { get; set; } = "Student";

    [TempData]
    public string? ErrorMessage { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public async Task OnGetAsync(
        string? returnUrl = null,
        string? role = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(
                string.Empty,
                ErrorMessage);
        }

        returnUrl ??= Url.Content("~/");

        await HttpContext.SignOutAsync(
            IdentityConstants.ExternalScheme);

        ExternalLogins =
            (await _signInManager
                .GetExternalAuthenticationSchemesAsync())
            .ToList();

        ReturnUrl = returnUrl;

        SelectedRole = NormalizeRole(role);
    }

    public async Task<IActionResult> OnPostAsync(
        string? returnUrl = null,
        string? role = null)
    {
        returnUrl ??= Url.Content("~/");

        ExternalLogins =
            (await _signInManager
                .GetExternalAuthenticationSchemesAsync())
            .ToList();

        SelectedRole = NormalizeRole(role);

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager
            .FindByEmailAsync(Input.Email);

        if (user == null)
        {
            ModelState.AddModelError(
                string.Empty,
                "Invalid login attempt.");

            return Page();
        }

        var userRoles =
            await _userManager.GetRolesAsync(user);

        if (!userRoles.Contains(SelectedRole))
        {
            ModelState.AddModelError(
                string.Empty,
                $"This account is not registered as a {SelectedRole}.");

            return Page();
        }

        var result =
            await _signInManager.PasswordSignInAsync(
                Input.Email,
                Input.Password,
                Input.RememberMe,
                lockoutOnFailure: false);

        if (result.Succeeded)
        {
            _logger.LogInformation(
                "User logged in as {Role}.",
                SelectedRole);

            if (SelectedRole == "Student")
            {
                return RedirectToAction(
                    "Index",
                    "Student");
            }

            if (SelectedRole == "TPO")
            {
                return RedirectToAction(
                    "Index",
                    "TPO");
            }

            if (SelectedRole == "Company")
            {
                return RedirectToAction(
                    "Index",
                    "Company");
            }

            return RedirectToAction(
                "Index",
                "Home");
        }

        if (result.RequiresTwoFactor)
        {
            return RedirectToPage(
                "./LoginWith2fa",
                new
                {
                    ReturnUrl = returnUrl,
                    RememberMe = Input.RememberMe,
                    role = SelectedRole
                });
        }

        if (result.IsLockedOut)
        {
            _logger.LogWarning(
                "User account locked out.");

            return RedirectToPage("./Lockout");
        }

        ModelState.AddModelError(
            string.Empty,
            "Invalid login attempt.");

        return Page();
    }

    private static string NormalizeRole(string? role)
    {
        if (string.Equals(
                role,
                "TPO",
                StringComparison.OrdinalIgnoreCase))
        {
            return "TPO";
        }

        if (string.Equals(
                role,
                "Company",
                StringComparison.OrdinalIgnoreCase))
        {
            return "Company";
        }

        return "Student";
    }
}