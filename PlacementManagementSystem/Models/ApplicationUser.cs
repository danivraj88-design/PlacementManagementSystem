using Microsoft.AspNetCore.Identity;

namespace PlacementManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}