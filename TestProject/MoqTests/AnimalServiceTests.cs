using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyDomain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeestjeFeestje.Services;
using BeestjeFeestje.DbAccess;

namespace TestProject.MoqTests
{
    [TestClass]
    public class AnimalServiceTests
    {
        private Mock<MyAnimalDbContext> _mockDbContext;
        private Mock<DbSet<Animal>> _mockAnimalDbSet;
        private AnimalService _animalService;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockDbContext = new Mock<MyAnimalDbContext>();
            _mockAnimalDbSet = new Mock<DbSet<Animal>>();

            // Set up DbSet behavior for Animals
            var animals = new List<Animal>
            {
                new Animal { Id = 1, Name = "Lion", Type = "Wild", Price = 100, Image = "lion.jpg" },
                new Animal { Id = 2, Name = "Elephant", Type = "Wild", Price = 150, Image = "elephant.jpg" }
            }.AsQueryable();

            _mockAnimalDbSet.As<IQueryable<Animal>>().Setup(m => m.Provider).Returns(animals.Provider);
            _mockAnimalDbSet.As<IQueryable<Animal>>().Setup(m => m.Expression).Returns(animals.Expression);
            _mockAnimalDbSet.As<IQueryable<Animal>>().Setup(m => m.ElementType).Returns(animals.ElementType);
            _mockAnimalDbSet.As<IQueryable<Animal>>().Setup(m => m.GetEnumerator()).Returns(animals.GetEnumerator());

            _mockDbContext.Setup(c => c.Animals).Returns(_mockAnimalDbSet.Object);

            _mockAnimalDbSet.Setup(m => m.Remove(It.IsAny<Animal>())).Callback<Animal>(a =>
            {
                var animalToRemove = animals.FirstOrDefault(x => x.Id == a.Id);
                if (animalToRemove != null)
                {
                    animals.ToList().Remove(animalToRemove);
                }

                _mockAnimalDbSet.As<IQueryable<Animal>>().Setup(m => m.GetEnumerator()).Returns(animals.GetEnumerator());
            });

            _animalService = new AnimalService(_mockDbContext.Object);
        }

        [TestMethod]
        public async Task GetAllAnimalsAsync_Should_ReturnAllAnimals()
        {
            // Act
            var result = await _animalService.GetAllAnimalsAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Lion", result[0].Name);
            Assert.AreEqual("Elephant", result[1].Name);
        }

        [TestMethod]
        public async Task GetAnimalByIdAsync_Should_ReturnCorrectAnimal_WhenAnimalExists()
        {
            // Act
            var result = await _animalService.GetAnimalByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Lion", result.Name);
        }

        [TestMethod]
        public async Task GetAnimalByIdAsync_Should_ReturnNull_WhenAnimalDoesNotExist()
        {
            // Act
            var result = await _animalService.GetAnimalByIdAsync(99);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task CreateAnimalAsync_Should_AddAnimalToDbContext()
        {
            // Arrange
            var newAnimal = new Animal { Id = 3, Name = "Tiger", Type = "Wild", Price = 120, Image = "tiger.jpg" };

            _mockAnimalDbSet.Setup(m => m.Add(It.IsAny<Animal>())).Callback<Animal>(a =>
            {
                var animals = _mockAnimalDbSet.Object.ToList();
                animals.Add(a);
                _mockAnimalDbSet.As<IQueryable<Animal>>().Setup(m => m.GetEnumerator()).Returns(animals.GetEnumerator());
            });

            // Act
            var result = await _animalService.CreateAnimalAsync(newAnimal);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task UpdateAnimalAsync_Should_UpdateExistingAnimal_WhenAnimalExists()
        {
            var newAnimal = new Animal { Id = 1, Name = "Lion", Type = "Wild", Price = 110, Image = "lion.jpg" };

            _mockAnimalDbSet.Setup(m => m.Add(It.IsAny<Animal>())).Callback<Animal>(a =>
            {
                var animals = _mockAnimalDbSet.Object.ToList();
                animals.Add(a);
                _mockAnimalDbSet.As<IQueryable<Animal>>().Setup(m => m.GetEnumerator()).Returns(animals.GetEnumerator());
            });

            // Act
            var result1 = await _animalService.CreateAnimalAsync(newAnimal);
            // Arrange
            var updatedAnimal = new Animal { Id = 1, Name = "Updated Lion", Type = "Wild", Price = 110, Image = "updated_lion.jpg" };

            // Act
            var result2 = await _animalService.UpdateAnimalAsync(updatedAnimal);

            // Assert
            Assert.IsTrue(result2);
            Assert.AreEqual("Updated Lion", _mockAnimalDbSet.Object.First(a => a.Id == 1).Name);
        }

        [TestMethod]
        public async Task UpdateAnimalAsync_Should_ReturnFalse_WhenAnimalDoesNotExist()
        {
            // Arrange
            var nonExistentAnimal = new Animal { Id = 99, Name = "NonExistent", Type = "Unknown", Price = 0, Image = "none.jpg" };

            // Act
            var result = await _animalService.UpdateAnimalAsync(nonExistentAnimal);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task DeleteAnimalAsync_Should_RemoveAnimal_WhenAnimalExists()
        {
            // Act
            var result = await _animalService.DeleteAnimalAsync(1);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DeleteAnimalAsync_Should_ReturnFalse_WhenAnimalDoesNotExist()
        {
            // Act
            var result = await _animalService.DeleteAnimalAsync(99);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
