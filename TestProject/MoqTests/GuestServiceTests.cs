using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyDomain;
using MyDomain.Guest;
using BeestjeFeestje.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.MoqTests
{
    [TestClass]
    public class GuestServiceTests
    {
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private GuestService _guestService;

        [TestInitialize]
        public void SetUp()
        {
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                null, null, null, null, null, null, null, null);

            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                null, null, null, null);

            _guestService = new GuestService(_userManagerMock.Object, _roleManagerMock.Object);
        }

        [TestMethod]
        public async Task CreateGuestAsync_SuccessfullyCreatesGuest_ReturnsTrueAndGeneratedPassword()
        {
            // Arrange
            var mockGuestVM = new Mock<IGuestVM>();
            mockGuestVM.SetupGet(m => m.Name).Returns("Test Guest");
            mockGuestVM.SetupGet(m => m.Email).Returns("guest@example.com");
            mockGuestVM.SetupGet(m => m.PhoneNumber).Returns("1234567890");
            mockGuestVM.SetupGet(m => m.Address).Returns("123 Test St.");
            mockGuestVM.SetupGet(m => m.CustomerCard).Returns("Card123");

            _userManagerMock
                .Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _roleManagerMock
                .Setup(rm => rm.RoleExistsAsync("Guest"))
                .ReturnsAsync(false);

            _roleManagerMock
                .Setup(rm => rm.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Guest"))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _guestService.CreateGuestAsync(mockGuestVM.Object);

            // Assert
            Assert.IsTrue(result.Succeeded);
            Assert.IsNotNull(result.Password);
            Assert.IsNull(result.ErrorMessage);
            _userManagerMock.Verify(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            _roleManagerMock.Verify(rm => rm.RoleExistsAsync("Guest"), Times.Once);
            _roleManagerMock.Verify(rm => rm.CreateAsync(It.IsAny<IdentityRole>()), Times.Once);
            _userManagerMock.Verify(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Guest"), Times.Once);
        }

        [TestMethod]
        public async Task CreateGuestAsync_FailsToCreateUser_ReturnsFalseAndErrorMessage()
        {
            // Arrange
            var mockGuestVM = new Mock<IGuestVM>();
            mockGuestVM.SetupGet(m => m.Name).Returns("Test Guest");
            mockGuestVM.SetupGet(m => m.Email).Returns("guest@example.com");
            mockGuestVM.SetupGet(m => m.PhoneNumber).Returns("1234567890");
            mockGuestVM.SetupGet(m => m.Address).Returns("123 Test St.");
            mockGuestVM.SetupGet(m => m.CustomerCard).Returns("Zilver");

            _userManagerMock
                .Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error creating user." }));

            // Act
            var result = await _guestService.CreateGuestAsync(mockGuestVM.Object);

            // Assert
            Assert.IsFalse(result.Succeeded);
            Assert.IsNull(result.Password);
            Assert.AreEqual("Error creating user.", result.ErrorMessage);
            _userManagerMock.Verify(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            _roleManagerMock.Verify(rm => rm.RoleExistsAsync(It.IsAny<string>()), Times.Never);
            _userManagerMock.Verify(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }
        [TestMethod]
        public async Task GetAllGuestsAsync_ReturnsListOfGuests()
        {
            // Arrange
            var mockUsers = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1", Name = "Guest 1", Email = "guest1@example.com" },
                new ApplicationUser { Id = "2", Name = "Guest 2", Email = "guest2@example.com" }
            };

            _userManagerMock
                .Setup(um => um.GetUsersInRoleAsync("Guest"))
                .ReturnsAsync(mockUsers);

            // Act
            var result = await _guestService.GetAllGuestsAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Guest 1", result[0].Name);
            Assert.AreEqual("Guest 2", result[1].Name);
            _userManagerMock.Verify(um => um.GetUsersInRoleAsync("Guest"), Times.Once);
        }

        [TestMethod]
        public async Task GetGuestByIdAsync_GuestExists_ReturnsGuest()
        {
            // Arrange
            var mockUser = new ApplicationUser
            {
                Id = "1",
                Name = "Test Guest",
                Email = "guest@example.com",
                PhoneNumber = "1234567890"
            };

            _userManagerMock
                .Setup(um => um.FindByIdAsync("1"))
                .ReturnsAsync(mockUser);

            _userManagerMock
                .Setup(um => um.IsInRoleAsync(mockUser, "Guest"))
                .ReturnsAsync(true);

            // Act
            var result = await _guestService.GetGuestByIdAsync("1");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test Guest", result.Name);
            Assert.AreEqual("guest@example.com", result.Email);
            _userManagerMock.Verify(um => um.FindByIdAsync("1"), Times.Once);
            _userManagerMock.Verify(um => um.IsInRoleAsync(mockUser, "Guest"), Times.Once);
        }

        [TestMethod]
        public async Task GetGuestByIdAsync_GuestDoesNotExist_ReturnsNull()
        {
            // Arrange
            _userManagerMock
                .Setup(um => um.FindByIdAsync("1"))
                .ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _guestService.GetGuestByIdAsync("1");

            // Assert
            Assert.IsNull(result);
            _userManagerMock.Verify(um => um.FindByIdAsync("1"), Times.Once);
        }

        [TestMethod]
        public async Task EditGuestAsync_GuestExists_SuccessfullyEditsGuest()
        {
            // Arrange
            var mockUser = new ApplicationUser { Id = "1", Name = "Old Name", Email = "old@example.com" };
            var mockGuestVM = new Mock<IGuestVM>();
            mockGuestVM.SetupGet(m => m.Id).Returns("1");
            mockGuestVM.SetupGet(m => m.Name).Returns("New Name");
            mockGuestVM.SetupGet(m => m.Email).Returns("new@example.com");

            _userManagerMock
                .Setup(um => um.FindByIdAsync("1"))
                .ReturnsAsync(mockUser);

            _userManagerMock
                .Setup(um => um.UpdateAsync(mockUser))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _guestService.EditGuestAsync(mockGuestVM.Object);

            // Assert
            Assert.IsTrue(result.Succeeded);
            Assert.IsNull(result.ErrorMessage);
            Assert.AreEqual("New Name", mockUser.Name);
            Assert.AreEqual("new@example.com", mockUser.Email);
            _userManagerMock.Verify(um => um.FindByIdAsync("1"), Times.Once);
            _userManagerMock.Verify(um => um.UpdateAsync(mockUser), Times.Once);
        }

        [TestMethod]
        public async Task EditGuestAsync_GuestDoesNotExist_ReturnsError()
        {
            // Arrange
            var mockGuestVM = new Mock<IGuestVM>();
            mockGuestVM.SetupGet(m => m.Id).Returns("1");

            _userManagerMock
                .Setup(um => um.FindByIdAsync("1"))
                .ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _guestService.EditGuestAsync(mockGuestVM.Object);

            // Assert
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Guest not found.", result.ErrorMessage);
            _userManagerMock.Verify(um => um.FindByIdAsync("1"), Times.Once);
        }

        [TestMethod]
        public async Task DeleteGuestAsync_GuestExists_SuccessfullyDeletesGuest()
        {
            // Arrange
            var mockUser = new ApplicationUser { Id = "1" };

            _userManagerMock
                .Setup(um => um.FindByIdAsync("1"))
                .ReturnsAsync(mockUser);

            _userManagerMock
                .Setup(um => um.DeleteAsync(mockUser))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _guestService.DeleteGuestAsync("1");

            // Assert
            Assert.IsTrue(result.Succeeded);
            Assert.IsNull(result.ErrorMessage);
            _userManagerMock.Verify(um => um.FindByIdAsync("1"), Times.Once);
            _userManagerMock.Verify(um => um.DeleteAsync(mockUser), Times.Once);
        }

        [TestMethod]
        public async Task DeleteGuestAsync_GuestDoesNotExist_ReturnsError()
        {
            // Arrange
            _userManagerMock
                .Setup(um => um.FindByIdAsync("1"))
                .ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _guestService.DeleteGuestAsync("1");

            // Assert
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Guest not found.", result.ErrorMessage);
            _userManagerMock.Verify(um => um.FindByIdAsync("1"), Times.Once);
        }
    }
}
