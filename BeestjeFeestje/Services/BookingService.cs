﻿using BeestjeFeestje.DbAccess;
using Microsoft.EntityFrameworkCore;
using MyDomain;
using Newtonsoft.Json;
using BeestjeFeestje.Models;
using MyDomain.Guest;

namespace BeestjeFeestje.Services
{
    public class BookingService : IBookingService
    {
        private readonly MyAnimalDbContext _context;

        public BookingService(MyAnimalDbContext context)
        {
            _context = context;
        }

        public async Task<BookingViewModel> GetBookingViewModelAsync(DateTime selectedDate)
        {
            var animals = _context.Animals.ToList();
            var bookings = _context.Bookings
                .Include(b => b.BookingAnimals)
                .ThenInclude(ba => ba.Animal)
                .ToList();

            var animalsFromOtherBookings = bookings
                .Where(b => b.BookedDate.Date == selectedDate)
                .SelectMany(b => b.BookingAnimals.Select(ba => ba.Animal))
                .ToList();

            var availableAnimals = animals.Where(animal => !animalsFromOtherBookings.Contains(animal)).ToList();

            return new BookingViewModel
            {
                SelectedDate = selectedDate,
                Animals = availableAnimals
            };
        }

        public async Task<BookingViewModel> BookAnimalAsync(BookingViewModel model, List<int> selectedAnimalIds)
        {
            if (selectedAnimalIds == null || !selectedAnimalIds.Any())
            {
                return model;
            }

            var selectedAnimals = _context.Animals.Where(a => selectedAnimalIds.Contains(a.Id)).ToList();
            model.Animals = selectedAnimals;
            return model;
        }

        public async Task SaveBookingToSessionAsync(BookingViewModel model, string animals, HttpContext context)
        {
            if (!string.IsNullOrEmpty(animals))
            {
                model.Animals = JsonConvert.DeserializeObject<List<Animal>>(animals);
            }
            context.Session.SetString("BookingSession", JsonConvert.SerializeObject(model));
            await Task.CompletedTask;
        }

        public async Task<BookingViewModel> GetBookingFromSessionAsync(HttpContext context)
        {
            var sessionBookingData = context.Session.GetString("BookingSession");
            var sessionUserData = context.Session.GetString("UserSession");

            if (sessionBookingData == null)
            {
                return null;
            }

            var model = JsonConvert.DeserializeObject<BookingViewModel>(sessionBookingData);

            if (model.Guest == null && sessionUserData != null)
            {
                var user = JsonConvert.DeserializeObject<ApplicationUser>(sessionUserData);
                if (user != null)
                {
                    model.Guest = new GuestVM
                    {
                        Name = user.Name,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Address = user.Address,
                        CustomerCard = user.CustomerCard
                    };
                }
            }

            return await Task.FromResult(model);
        }
        public void DeleteBookingAsync(HttpContext context)
        {
            context.Session.Remove("BookingSession");
        }

        public async Task<bool> AddBookingAsync(BookingViewModel model, string animals, HttpContext context)
        {
            string userId = null;
            var booking = new Booking();

            if (!string.IsNullOrEmpty(animals))
            {
                var selectedAnimals = JsonConvert.DeserializeObject<List<Animal>>(animals);
                booking.BookingAnimals = selectedAnimals.Select(a => new BookingAnimal { AnimalId = a.Id }).ToList();
            }

            var sessionUserData = context.Session.GetString("UserSession");
            if (sessionUserData != null)
            {
                var userModel = JsonConvert.DeserializeObject<ApplicationUser>(sessionUserData);
                userId = userModel.Id;
            }

            booking.BookedDate = model.SelectedDate;
            booking.UserId = userId;
            booking.ContactInformation = new ContactInformation
            {
                Name = model.Guest.Name,
                Email = model.Guest.Email,
                PhoneNumber = model.Guest.PhoneNumber,
                Address = model.Guest.Address
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            return _context.Bookings.ToList();
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            return _context.Bookings
                .Include(b => b.BookingAnimals)
                .ThenInclude(ba => ba.Animal)
                .FirstOrDefault(b => b.Id == id);
        }

        public async Task<bool> DeleteBookingByIdAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return false;
            }
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<List<ContactInformation>> GetContactPerBooking(int bookingId)
        {
            return await _context.Contacts.Where(c => c.BookingId == bookingId).ToListAsync();
        }

        public async Task<ApplicationUser> GetUserForBooking(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

    }
}
