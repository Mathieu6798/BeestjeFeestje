using Microsoft.AspNetCore.Identity;
using MyDomain;
using MyDomain.Guest;

namespace BeestjeFeestje.Services
{
    public class GuestService : IGuestService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public GuestService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<(bool Succeeded, string Password, string ErrorMessage)> CreateGuestAsync(IGuestVM model)
        {
            var user = new ApplicationUser
            {
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                CustomerCard = model.CustomerCard,
                UserName = model.Address.Replace(" ", "_")
            };

            var password = GeneratePassword();
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return (false, null, string.Join("; ", result.Errors.Select(e => e.Description)));
            }

            if (!await _roleManager.RoleExistsAsync("Guest"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Guest"));
            }
            await _userManager.AddToRoleAsync(user, "Guest");
            return (true, password, null);
        }

        private string GeneratePassword()
        {
            int length = 10;
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";
            var random = new Random();
            string password = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            while (!password.Any(char.IsDigit) || !password.Any(char.IsUpper))
            {
                password = new string(Enumerable.Repeat(chars, length)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            return password;
        }
        public async Task<List<IGuestVM>> GetAllGuestsAsync()
        {
            // Get all users in the role "Guest"
            List<IGuestVM> result = new List<IGuestVM>();
            var users = await _userManager.GetUsersInRoleAsync("Guest");

            foreach(var guest in users)
            {
                IGuestVM guestVM = new GuestVM
                {
                    Id = guest.Id,
                    Name = guest.Name,
                    Email = guest.Email,
                    PhoneNumber = guest.PhoneNumber,
                    Address = guest.Address,
                    CustomerCard = guest.CustomerCard
                };
                result.Add(guestVM);
            }

            return result;
        }
        public async Task<IGuestVM> GetGuestByIdAsync(string id)
        {
            // Find the user by their ID
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null; 
            }

            // Check if the user has the "Guest" role
            var isGuest = await _userManager.IsInRoleAsync(user, "Guest");
            if (!isGuest)
            {
                return null; 
            }

            IGuestVM guestVM = new GuestVM
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                CustomerCard = user.CustomerCard
            };

            return guestVM;
        }

        public async Task<(bool Succeeded, string ErrorMessage)> EditGuestAsync(IGuestVM model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return (false, "Guest not found.");
            }

            user.Name = model.Name;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            user.CustomerCard = model.CustomerCard;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return (false, string.Join("; ", result.Errors.Select(e => e.Description)));
            }

            return (true, null);
        }

        public async Task<(bool Succeeded, string ErrorMessage)> DeleteGuestAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return (false, "Guest not found.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return (false, string.Join("; ", result.Errors.Select(e => e.Description)));
            }

            return (true, null);
        }
    }
}
