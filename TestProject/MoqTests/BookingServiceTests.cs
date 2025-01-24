using BeestjeFeestje.Models;
using BeestjeFeestje.Services;
using DbAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyDomain;
using MyDomain.Guest;
using Moq;
using TestProject.MoqTests;
using System.Data.Entity.Infrastructure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;

namespace TestProject.MoqTests
{
    [TestClass]
    public class BookingServiceTests
    {
        private Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private Mock<ISession> _mockSession;
        private Mock<MyAnimalDbContext> _mockDbContext;
        private BookingService _bookingService;
        public BookingServiceTests() { }

        [TestInitialize]
        public void TestInitialize()
        {
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockSession = new Mock<ISession>();
            _mockDbContext = new Mock<MyAnimalDbContext>();

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.Session).Returns(_mockSession.Object);
            _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext.Object);

            // Mock Session storage
            var sessionStorage = new Dictionary<string, byte[]>();
            _mockSession.Setup(s => s.TryGetValue(It.IsAny<string>(), out It.Ref<byte[]>.IsAny))
                .Returns((string key, out byte[] value) =>
                {
                    if (sessionStorage.TryGetValue(key, out var storedValue))
                    {
                        value = storedValue;
                        return true;
                    }

                    value = null;
                    return false;
                });

            _mockSession.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Callback<string, byte[]>((key, value) =>
                {
                    sessionStorage[key] = value;
                });

            _bookingService = new BookingService(_mockDbContext.Object);
        }

        [TestMethod]
        public async Task GetBookingViewModelAsyncTest()
        {
            // Arrange
            var animals = new List<Animal>
            {
                new Animal { Id = 1, Name = "Animal1", Price = 10 },
                new Animal { Id = 2, Name = "Animal2", Price = 20 }
            }.AsQueryable();

            var bookings = new List<Booking>
            {
                new Booking
                {
                    Id = 1,
                    BookedDate = DateTime.Today,
                    BookingAnimals = new List<BookingAnimal>
                    {
                        new BookingAnimal { Animal = animals.First() }
                    }
                }
            }.AsQueryable();

            var booking = new Booking
            {
                Id = 1,
                BookedDate = DateTime.Today,
                BookingAnimals = new List<BookingAnimal>
                    {
                        new BookingAnimal { Animal = animals.First() }
                    }
            };

            // Mock DbSet for Animals
            var mockAnimalDbSet = new Mock<DbSet<Animal>>();
            mockAnimalDbSet.As<IQueryable<Animal>>().Setup(m => m.Provider).Returns(animals.AsQueryable().Provider);
            mockAnimalDbSet.As<IQueryable<Animal>>().Setup(m => m.Expression).Returns(animals.AsQueryable().Expression);
            mockAnimalDbSet.As<IQueryable<Animal>>().Setup(m => m.ElementType).Returns(animals.AsQueryable().ElementType);
            mockAnimalDbSet.As<IQueryable<Animal>>().Setup(m => m.GetEnumerator()).Returns(animals.AsQueryable().GetEnumerator());
            _mockDbContext.Setup(c => c.Animals).Returns(mockAnimalDbSet.Object);

            // Mock DbSet for Bookings
            var mockBookingDbSet = new Mock<DbSet<Booking>>();
            mockBookingDbSet.As<IQueryable<Booking>>().Setup(m => m.Provider).Returns(bookings.AsQueryable().Provider);
            mockBookingDbSet.As<IQueryable<Booking>>().Setup(m => m.Expression).Returns(bookings.AsQueryable().Expression);
            mockBookingDbSet.As<IQueryable<Booking>>().Setup(m => m.ElementType).Returns(bookings.AsQueryable().ElementType);
            mockBookingDbSet.As<IQueryable<Booking>>().Setup(m => m.GetEnumerator()).Returns(bookings.AsQueryable().GetEnumerator());
            _mockDbContext.Setup(c => c.Bookings).Returns(mockBookingDbSet.Object);


            // Act
            var service = new BookingService(_mockDbContext.Object);
            var model = await service.GetBookingViewModelAsync(DateTime.Today);

            // Assert
            Assert.AreEqual(1, model.Animals.Count);
            Assert.AreEqual(2, model.Animals[0].Id);
        }
        [TestMethod]
        public async Task BookAnimalAsyncTest()
        {
            // Arrange
            var booking = new Booking
            {
                Id = 1,
                BookedDate = DateTime.Today,
                BookingAnimals = new List<BookingAnimal>
                {
                    new BookingAnimal { AnimalId = 1 }
                }
            };

            var animals = new List<Animal>
            {
                new Animal { Id = 1, Name = "Animal1", Price = 10 }
            }.AsQueryable();

            var mockAnimalDbSet = new Mock<DbSet<Animal>>();
            mockAnimalDbSet.As<IQueryable<Animal>>().Setup(m => m.Provider).Returns(animals.AsQueryable().Provider);
            mockAnimalDbSet.As<IQueryable<Animal>>().Setup(m => m.Expression).Returns(animals.AsQueryable().Expression);
            mockAnimalDbSet.As<IQueryable<Animal>>().Setup(m => m.ElementType).Returns(animals.AsQueryable().ElementType);
            mockAnimalDbSet.As<IQueryable<Animal>>().Setup(m => m.GetEnumerator()).Returns(animals.AsQueryable().GetEnumerator());
            _mockDbContext.Setup(c => c.Animals).Returns(mockAnimalDbSet.Object);

            _mockDbContext.Setup(c => c.Bookings.Add(It.IsAny<Booking>())).Callback<Booking>(b => booking = b);

            // Act
            var service = new BookingService(_mockDbContext.Object);
            var success = await service.BookAnimalAsync(new BookingViewModel(), new List<int> { 1 });

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(1, booking.BookingAnimals.Count);
            Assert.AreEqual(1, booking.BookingAnimals.First().AnimalId);
        }
        [TestMethod]
        public async Task SaveBookingToSessionAsync_Should_SaveSerializedModelToSession()
        {
            // Arrange
            var bookingViewModel = new BookingViewModel
            {
                SelectedDate = DateTime.Today,
                Animals = new List<Animal>
                {
                    new Animal { Id = 1, Name = "Animal1", Price = 10 },
                    new Animal { Id = 2, Name = "Animal2", Price = 20 }
                }
            };

            var serializedAnimals = JsonConvert.SerializeObject(bookingViewModel.Animals);
            var expectedSerializedModel = JsonConvert.SerializeObject(bookingViewModel);

            var sessionStorage = new Dictionary<string, byte[]>();
            _mockSession.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                        .Callback<string, byte[]>((key, value) => sessionStorage[key] = value);

            // Act
            await _bookingService.SaveBookingToSessionAsync(bookingViewModel, serializedAnimals, _mockHttpContextAccessor.Object.HttpContext);

            // Assert
            Assert.IsTrue(sessionStorage.ContainsKey("BookingSession"), "Session should contain 'BookingSession'.");

            var actualSerializedModel = Encoding.UTF8.GetString(sessionStorage["BookingSession"]);
            Assert.AreEqual(expectedSerializedModel, actualSerializedModel, "Serialized model in session does not match expected serialized model.");
        }
        [TestMethod]
        public async Task GetBookingFromSessionAsync_Should_ReturnBookingViewModel_WhenSessionDataExists()
        {
            // Arrange
            var bookingViewModel = new BookingViewModel
            {
                SelectedDate = DateTime.Today,
                Animals = new List<Animal>
                    {
                        new Animal { Id = 1, Name = "Animal1", Price = 10 }
                    },
                Guest = null
            };

            var user = new ApplicationUser
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123456789",
                Address = "123 Main St",
                CustomerCard = "ABC123"
            };

            var serializedBookingData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(bookingViewModel));
            var serializedUserData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(user));

            _mockSession.Object.Set("BookingSession", serializedBookingData);
            _mockSession.Object.Set("UserSession", serializedUserData);

            // Act
            var result = await _bookingService.GetBookingFromSessionAsync(_mockHttpContextAccessor.Object.HttpContext);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(DateTime.Today, result.SelectedDate);
            Assert.AreEqual(1, result.Animals.Count);
            Assert.AreEqual("Animal1", result.Animals[0].Name);
            Assert.IsNotNull(result.Guest);
            Assert.AreEqual("John Doe", result.Guest.Name);
            Assert.AreEqual("john.doe@example.com", result.Guest.Email);
        }

        [TestMethod]
        public async Task GetBookingFromSessionAsync_Should_ReturnNull_WhenBookingSessionDataIsNull()
        {

            // Act
            var result = await _bookingService.GetBookingFromSessionAsync(_mockHttpContextAccessor.Object.HttpContext);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetBookingFromSessionAsync_Should_HandleMissingUserSessionData()
        {
            // Arrange
            var bookingViewModel = new BookingViewModel
            {
                SelectedDate = DateTime.Today,
                Animals = new List<Animal>
                {
                    new Animal { Id = 1, Name = "Animal1", Price = 10 }
                },
                Guest = null
            };

            var serializedBookingData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(bookingViewModel));

            _mockSession.Object.Set("BookingSession", serializedBookingData);

            // Act
            var result = await _bookingService.GetBookingFromSessionAsync(_mockHttpContextAccessor.Object.HttpContext);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(DateTime.Today, result.SelectedDate);
            Assert.AreEqual(1, result.Animals.Count);
            Assert.AreEqual("Animal1", result.Animals[0].Name);
            Assert.IsNull(result.Guest);
        }
        [TestMethod]
        public async Task AddBookingAsync_Should_AddBookingAndReturnTrue()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<Booking>>();
            var bookings = new List<Booking>();
            mockDbSet.Setup(m => m.Add(It.IsAny<Booking>()))
                     .Callback<Booking>(b => bookings.Add(b));

            _mockDbContext.Setup(c => c.Bookings).Returns(mockDbSet.Object);

            var animalsJson = JsonConvert.SerializeObject(new List<Animal>
                {
                    new Animal { Id = 1, Name = "Animal1" },
                    new Animal { Id = 2, Name = "Animal2" }
                });

            var guest = new GuestVM
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123456789",
                Address = "123 Main St"
            };

            var model = new BookingViewModel
            {
                SelectedDate = DateTime.Today,
                Guest = guest
            };

            var user = new ApplicationUser { Id = "User123" };
            var sessionStorage = new Dictionary<string, byte[]>();

            _mockSession.Setup(s => s.TryGetValue("UserSession", out It.Ref<byte[]>.IsAny))
                         .Returns((string key, out byte[] value) =>
                         {
                             if (sessionStorage.TryGetValue(key, out var storedValue))
                             {
                                 value = storedValue;
                                 return true;
                             }

                             value = null;
                             return false;
                         });

            _mockSession.Setup(s => s.Set("UserSession", It.IsAny<byte[]>()));

            sessionStorage["UserSession"] = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(user));

            // Act
            var result = await _bookingService.AddBookingAsync(model, animalsJson, _mockHttpContextAccessor.Object.HttpContext);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, bookings.Count);

            var addedBooking = bookings.First();
            Assert.AreEqual(DateTime.Today, addedBooking.BookedDate);
            Assert.AreEqual("User123", addedBooking.UserId);
            Assert.AreEqual(guest.Name, addedBooking.ContactInformation.Name);
            Assert.AreEqual(guest.Email, addedBooking.ContactInformation.Email);
            Assert.AreEqual(guest.PhoneNumber, addedBooking.ContactInformation.PhoneNumber);
            Assert.AreEqual(guest.Address, addedBooking.ContactInformation.Address);

            Assert.AreEqual(2, addedBooking.BookingAnimals.Count);
            Assert.AreEqual(1, addedBooking.BookingAnimals.ToList()[0].AnimalId);
            Assert.AreEqual(2, addedBooking.BookingAnimals.ToList()[1].AnimalId);
        }
    }
}
