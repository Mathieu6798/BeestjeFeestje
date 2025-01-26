using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MyDomain;
using MyDomain.Account;
namespace BeestjeFeestje.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM Input)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                var result = await _signInManager.PasswordSignInAsync(user?.UserName, Input.Password, false, lockoutOnFailure: false);
                HttpContext.Session.SetString("UserSession", JsonConvert.SerializeObject(user));

                if (result.Succeeded)
                {
                    var sessionBookingData = HttpContext.Session.GetString("BookingSession");
                    if (sessionBookingData != null)
                    {
                        return RedirectToAction("Overview", "Booking");
                    }
                    else
                    {
                        return LocalRedirect("/");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(Input);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Remove("UserSession");
            return LocalRedirect("/Account/Login");
        }

    }
}
