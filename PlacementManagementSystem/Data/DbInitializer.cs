using Microsoft.AspNetCore.Identity;

namespace PlacementManagementSystem.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAsync(
            RoleManager<IdentityRole> roleManager)
        {
            string[] roles =
            {
                "Student",
                "TPO",
                "Recruiter"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(role));
                }
            }
        }
    }
}