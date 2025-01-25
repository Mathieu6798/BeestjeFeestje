using BeestjeFeestje.Models;
using MyDomain;

namespace BeestjeFeestje.Services
{
    public interface IBookingService
    {
        Task<BookingViewModel> GetBookingViewModelAsync(DateTime selectedDate);
        Task<bool> BookAnimalAsync(BookingViewModel model, List<int> selectedAnimalIds);
        Task SaveBookingToSessionAsync(BookingViewModel model, string animals, HttpContext context);
        void DeleteBookingAsync(HttpContext context);
        Task<BookingViewModel> GetBookingFromSessionAsync(HttpContext context);
        Task<bool> AddBookingAsync(BookingViewModel model, string animals, HttpContext context);
        Task<List<Booking>> GetAllBookingsAsync();
        Task<Booking> GetBookingByIdAsync(int id);
        Task<bool> DeleteBookingByIdAsync(int id);
        Task<List<ContactInformation>> GetContactPerBooking(int bookingId);
        Task<ApplicationUser> GetUserForBooking(string userId);
    }
}
