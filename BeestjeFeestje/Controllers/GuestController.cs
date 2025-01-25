using BeestjeFeestje.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyDomain.Guest;
using MyDomain;
namespace BeestjeFeestje.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GuestController : Controller
    {
        private readonly IGuestService _guestService;

        public GuestController(IGuestService guestService)
        {
            _guestService = guestService;
        }

        public async Task<IActionResult> Index()
        {
            // Get all guests
            var guestUsers = await _guestService.GetAllGuestsAsync();
            return View(guestUsers);
        }

        public IActionResult Create()
        {
            GuestVM guestVM = new GuestVM();
            return View(guestVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GuestVM model)
        {
            var (succeeded, password, errorMessage) = await _guestService.CreateGuestAsync(model);

            if (succeeded)
            {
                TempData["SuccessMessage"] = "Guest added successfully!";
                TempData["Password"] = "The password of the guest is: " + password;
                return View(model);
            }

            TempData["ErrorMessage"] = errorMessage;
            return View(model);
        }
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _guestService.GetGuestByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            IGuestVM model = new GuestVM
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                CustomerCard = user.CustomerCard,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GuestVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (succeeded, errorMessage) = await _guestService.EditGuestAsync(model);

            if (succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = errorMessage;
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            IGuestVM guest = await _guestService.GetGuestByIdAsync(id);
            if (guest == null)
            {
                return NotFound();
            }

            return View(guest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var (succeeded, errorMessage) = await _guestService.DeleteGuestAsync(id);

            if (succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, errorMessage);
            return RedirectToAction(nameof(Delete), new { id });
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _guestService.GetGuestByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            IGuestVM model = new GuestVM
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                CustomerCard = user.CustomerCard,
            };

            return View(model);
        }
    }
}
