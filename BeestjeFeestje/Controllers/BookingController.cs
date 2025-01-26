using BeestjeFeestje.Models;
using BeestjeFeestje.Services;
using Microsoft.AspNetCore.Mvc;
using MyDomain;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

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
            var booking = await _bookingService.BookAnimalAsync(model, selectedAnimalIds);
            var user = HttpContext.Session.GetString("UserSession");
            if (booking.Animals == null)
            {
                TempData["ErrorMessage"] = "No animals selected.";
                return RedirectToAction(nameof(Index));
            }
            else if (model.Validate(new ValidationContext(model)).Any())
            {
                var validationErrors = model.Validate(new ValidationContext(model));
                TempData["ErrorMessage"] = validationErrors.First().ErrorMessage;
                return RedirectToAction(nameof(Index));
            }
            else if(user != null)
            {
                HttpContext.Session.SetString("BookingSession", JsonConvert.SerializeObject(booking));
                return RedirectToAction(nameof(Overview));
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
            if (model.Validate(new ValidationContext(model)).Any())
            {
                var validationErrors = model.Validate(new ValidationContext(model));
                TempData["ErrorMessage"] = validationErrors.First().ErrorMessage;
                return RedirectToAction(nameof(Index));
            }
            List<string> rules = model?.CalculateDiscount();
            TempData["DiscountRules"] = rules;  
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

        public async Task<IActionResult> BookingsList()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            foreach (var booking in bookings)
            {
                var contact = await _bookingService.GetContactPerBooking(booking.Id);
                booking.ContactInformation = contact.FirstOrDefault();
            }
            return View(bookings);
        }

        public async Task<IActionResult> Details(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);

            var contact = await _bookingService.GetContactPerBooking(booking.Id);


            if (contact != null)
            {
                booking.ContactInformation = contact.FirstOrDefault();
            }
            else if (booking.UserId != null)
            {
                booking.User = await _bookingService.GetUserForBooking(booking.UserId);
            }
            else if (booking.UserId == null && booking.ContactInformation == null)
            {
                TempData["ErrorMessage"] = "No contact information or user found for this booking.";
                return RedirectToAction(nameof(BookingsList));
            }

            if (booking == null) return NotFound();
            return View(booking);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);

            var contact = await _bookingService.GetContactPerBooking(booking.Id);


            if (contact != null)
            {
                booking.ContactInformation = contact.FirstOrDefault();
            }
            else if (booking.UserId != null)
            {
                booking.User = await _bookingService.GetUserForBooking(booking.UserId);
            }
            else if (booking.UserId == null && booking.ContactInformation == null)
            {
                TempData["ErrorMessage"] = "No contact information or user found for this booking.";
                return RedirectToAction(nameof(BookingsList));
            }

            if (booking == null) return NotFound();
            return View(booking);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bookingService.DeleteBookingByIdAsync(id);
            return RedirectToAction(nameof(BookingsList));
        }
    }
}
