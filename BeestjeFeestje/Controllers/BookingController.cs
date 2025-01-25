using BeestjeFeestje.Models;
using BeestjeFeestje.Services;
using Microsoft.AspNetCore.Mvc;
using MyDomain;

namespace BeestjeFeestje.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime? selectedDate)
        {
            var date = selectedDate ?? DateTime.Today;
            var model = await _bookingService.GetBookingViewModelAsync(date);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BookingViewModel model)
        {
            var viewModel = await _bookingService.GetBookingViewModelAsync(model.SelectedDate);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> BookAnimal(BookingViewModel model, List<int> selectedAnimalIds)
        {
            var success = await _bookingService.BookAnimalAsync(model, selectedAnimalIds);
            if (!success)
            {
                ModelState.AddModelError("", "No animals selected.");
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Animals booked successfully!";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(BookingViewModel model, string animals)
        {
            await _bookingService.SaveBookingToSessionAsync(model, animals, HttpContext);
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookingViewModel model, string animals)
        {
            await _bookingService.SaveBookingToSessionAsync(model, animals, HttpContext);
            return RedirectToAction("Overview");
        }

        public async Task<IActionResult> Overview()
        {
            var model = await _bookingService.GetBookingFromSessionAsync(HttpContext);
            model?.CalculateDiscount();
            _bookingService.DeleteBookingAsync(HttpContext);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking(BookingViewModel model, string animals)
        {
            var success = await _bookingService.AddBookingAsync(model, animals, HttpContext);
            if (success)
            {
                TempData["SuccessMessage"] = "Booking added successfully!";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Failed to add booking.";
            return RedirectToAction(nameof(Overview));
        }
    }
}
