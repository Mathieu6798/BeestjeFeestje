using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDomain
{
    public static class UserAndRoleSeeder
    {
        public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            // Set the roles
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "Admin"
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Guest").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "Guest"
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            // Create admin account
            if (userManager.FindByNameAsync("FarmAdmin").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "FarmAdmin",
                    Email = "Boerderij@localhost.nl",
                    Name = "Admin Name",
                    Address = "Admin Address"
                    //CustomerCard = "AdminCard123"
                };
                //Password
                IdentityResult result = userManager.CreateAsync(user, "Boerderij123!").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }

    }
}
