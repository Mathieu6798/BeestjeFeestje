using BeestjeFeestje.Models;
using MyDomain;

namespace BeestjeFeestje.Services
{
    public interface IBookingService
    {
        Task<BookingViewModel> GetBookingViewModelAsync(DateTime selectedDate);
        Task<bool> BookAnimalAsync(BookingViewModel model, List<int> selectedAnimalIds);
        Task SaveBookingToSessionAsync(BookingViewModel model, string animals, HttpContext context);
        Task<BookingViewModel> GetBookingFromSessionAsync(HttpContext context);
        Task<bool> AddBookingAsync(BookingViewModel model, string animals, HttpContext context);
    }
}
