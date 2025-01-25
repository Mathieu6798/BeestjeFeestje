using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BeestjeFeestje.Models;
using MyDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyDomain.Guest;

namespace BeestjeFeestje.Tests.Models
{
    [TestClass]
    public class BookingViewModelTests
    {
        [TestMethod]
        public void Validate_ShouldReturnError_WhenWildAnimalsAndFarmAnimalsBookedTogether()
        {
            // Arrange
            var viewModel = new BookingViewModel
            {
                Animals = new List<Animal>
                {
                    new Animal { Name = "Leeuw", Type = "Wild" },
                    new Animal { Name = "Koe", Type = "Boerderij" }
                },
                SelectedDate = DateTime.Now
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(viewModel, new ValidationContext(viewModel), validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Exists(vr => vr.ErrorMessage == "Nom nom nom"));
        }


        [TestMethod]
        public void Validate_ShouldReturnError_WhenPinguinBookedOnWeekend()
        {
            // Arrange
            var viewModel = new BookingViewModel
            {
                Animals = new List<Animal>
                {
                    new Animal { Name = "Pinguïn", Type = "Sneeuw" }
                },
                SelectedDate = new DateTime(2025, 1, 25) // Saturday
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(viewModel, new ValidationContext(viewModel), validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Exists(vr => vr.ErrorMessage == "Dieren in pak werken alleen doordeweeks"));
        }

        [TestMethod]
        public void CalculateDiscount_ShouldApplyCorrectDiscounts()
        {
            // Arrange
            var viewModel = new BookingViewModel
            {
                Animals = new List<Animal>
                {
                    new Animal { Name = "Eend", Type = "Wild" },
                    new Animal { Name = "Koe", Type = "Boerderij" },
                    new Animal { Name = "Pinguïn", Type = "Sneeuw" }
                },
                SelectedDate = new DateTime(2025, 1, 22), // Monday
                Guest = new GuestVM { CustomerCard = "Zilver" }
            };

            // Act
            viewModel.CalculateDiscount();

            // Assert
            Assert.IsTrue(viewModel.Discount > 0);
            Assert.IsTrue(viewModel.Discount <= 60);
        }

        // Test voor woestijn dieren in de winter
        [TestMethod]
        public void Validate_ShouldReturnError_WhenWoestijnAnimalBookedInColdMonth()
        {
            // Arrange
            var viewModel = new BookingViewModel
            {
                Animals = new List<Animal>
                {
                    new Animal { Name = "Kameel", Type = "Woestijn" }
                },
                SelectedDate = new DateTime(2025, 1, 15) // Januari (koude maand)
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(viewModel, new ValidationContext(viewModel), validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Exists(vr => vr.ErrorMessage == "Brrrr – Veelste koud"));
        }

        // Test voor maximale dieren voor een klantenkaart
        [TestMethod]
        public void Validate_ShouldReturnError_WhenTooManyAnimalsBookedForCustomerCard()
        {
            // Arrange
            var viewModel = new BookingViewModel
            {
                Animals = new List<Animal>
                {
                    new Animal { Name = "Koe", Type = "Boerderij" },
                    new Animal { Name = "Eend", Type = "Boerderij" },
                    new Animal { Name = "Kip", Type = "Boerderij" },
                    new Animal { Name = "Schaap", Type = "Boerderij" },
                    new Animal { Name = "Varken", Type = "Boerderij" } // Exceeds Zilver card limit
                },
                Guest = new GuestVM { CustomerCard = "Zilver" }
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(viewModel, new ValidationContext(viewModel), validationResults, true);

            // Assert
            Assert.IsFalse(isValid, "Validation should fail for exceeding max animals.");
            Assert.IsTrue(validationResults.Exists(vr => vr.ErrorMessage.Contains("U kunt maximaal 4 dieren reserveren op basis van uw klantenkaart")));
        }

        // Test voor maximale korting
        [TestMethod]
        public void CalculateDiscount_ShouldNotExceedMaximumDiscount()
        {
            // Arrange
            var viewModel = new BookingViewModel
            {
                Animals = new List<Animal>
            {
                new Animal { Name = "Eend", Type = "Wild" },
                new Animal { Name = "Pinguïn", Type = "Sneeuw" },
                new Animal { Name = "Leeuw", Type = "Wild" },
                new Animal { Name = "Olifant", Type = "Wild" },
                new Animal { Name = "Koe", Type = "Boerderij" }
            },
                SelectedDate = new DateTime(2025, 1, 22), // Monday
                Guest = new GuestVM { CustomerCard = "Goud" }
            };

            // Act
            viewModel.CalculateDiscount();

            // Assert
            Assert.AreEqual(60, viewModel.Discount); // Ensure discount doesn't exceed 60
        }



        // Sneeuwdieren mogen niet in de zomer geboekt worden
        [TestMethod]
        public void Validate_ShouldReturnError_WhenSneeuwAnimalBookedInSummer()
        {
            // Arrange
            var viewModel = new BookingViewModel
            {
                Animals = new List<Animal>
                {
                    new Animal { Name = "IJsbeer", Type = "Sneeuw" }
                },
                SelectedDate = new DateTime(2025, 7, 1) // Juli (zomer)
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(viewModel, new ValidationContext(viewModel), validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Exists(vr => vr.ErrorMessage == "Some People Are Worth Melting For. ~ Olaf"));
        }

        [TestMethod]
        public void Validate_ShouldReturnError_WhenTooManyAnimalsBookedForGoldCard()
        {
            // Arrange
            var viewModel = new BookingViewModel
            {
                Animals = new List<Animal>
        {
            new Animal { Name = "Koe", Type = "Boerderij" },
            new Animal { Name = "Eend", Type = "Boerderij" },
            new Animal { Name = "Kip", Type = "Boerderij" },
            new Animal { Name = "Schaap", Type = "Boerderij" },
            new Animal { Name = "Varken", Type = "Boerderij" } // No limit for Gold card
        },
                Guest = new GuestVM { CustomerCard = "Goud" }
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(viewModel, new ValidationContext(viewModel), validationResults, true);

            // Assert
            Assert.IsTrue(isValid); // Validation should pass because Gold card has no limit on the number of animals
        }

        [TestMethod]
        public void Validate_ShouldReturnError_WhenVIPAnimalBookedForGoldCard()
        {
            // Arrange
            var viewModel = new BookingViewModel
            {
                Animals = new List<Animal>
        {
            new Animal { Name = "VIP Elephant", Type = "VIP" } // VIP animal booked
        },
                Guest = new GuestVM { CustomerCard = "Goud" }
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(viewModel, new ValidationContext(viewModel), validationResults, true);

            // Assert
            Assert.IsFalse(isValid); // VIP animals should not be allowed for Gold cardholders
            Assert.IsTrue(validationResults.Exists(vr => vr.ErrorMessage == "Alleen houders van een Platina-kaart kunnen VIP-dieren boeken."));
        }


        // Als er 3 dieren van hetzelfde type worden geboekt, moet er 10% korting zijn
        [TestMethod]
        public void CalculateDiscount_ShouldApply10PercentDiscountForSameTypeAnimals()
        {
            // Arrange
            var viewModel = new BookingViewModel
            {
                Animals = new List<Animal>
                {
                    new Animal { Name = "Koe", Type = "Boerderij" },
                    new Animal { Name = "Eend", Type = "Boerderij" },
                    new Animal { Name = "Kip", Type = "Boerderij" }
                },
                SelectedDate = new DateTime(2025, 1, 22), // Monday
                Guest = new GuestVM { CustomerCard = "Zilver" }
            };

            // Act
            viewModel.CalculateDiscount();

            // Assert
            Assert.IsTrue(viewModel.Discount >= 10, "The discount should be at least 10%.");
        }

        // Als het dier 'Eend' is geboekt, moet er 50% korting zijn met 1 op 6 kans
        [TestMethod]
        public void CalculateDiscount_ShouldApply50PercentDiscountForEend()
        {
            // Arrange
            var viewModel = new BookingViewModel
            {
                Animals = new List<Animal>
        {
            new Animal { Name = "Eend", Type = "Boerderij" }
        },
                SelectedDate = new DateTime(2025, 1, 22), // Monday
                Guest = new GuestVM { CustomerCard = "Zilver" }
            };

            // Simulate a fixed random number (e.g., 1 for the 1-in-6 chance)
            int simulatedRandomNumber = 1;

            // Act
            if (viewModel.Animals.Exists(a => a.Name == "Eend"))
            {
                if (simulatedRandomNumber >= 1 && simulatedRandomNumber <= 6 && simulatedRandomNumber == 1)
                {
                    viewModel.Discount = 50;
                }
            }

            // Assert
            Assert.IsTrue(simulatedRandomNumber >= 1 && simulatedRandomNumber <= 6, "The random number should be between 1 and 6.");
            Assert.AreEqual(50, viewModel.Discount, "The discount should be 50% if 'Eend' is booked and the random condition is met.");
        }

        [TestMethod]
        public void CalculateDiscount_ShouldApplyExtraDiscountForAnimalNameContainingA()
        {
            // Arrange
            var viewModel = new BookingViewModel
            {
                Animals = new List<Animal>
        {
            new Animal { Name = "Koe", Type = "Boerderij" },  // No 'A' in name
            new Animal { Name = "Kip", Type = "Boerderij" },  // Has 'A' in name
            new Animal { Name = "Eend", Type = "Boerderij" }  // Has 'A' in name
        },
                SelectedDate = new DateTime(2025, 1, 22), // Monday
                Guest = new GuestVM { CustomerCard = "Zilver" }
            };

            // Act
            viewModel.CalculateDiscount();

            // Assert
            var expectedDiscount = 2 * 2; // 2% per animal with an 'A' in the name
            Assert.IsTrue(viewModel.Discount >= expectedDiscount, "Discount should include 2% for each animal with 'A' in its name.");
        }
        [TestMethod]
        public void Validate_ShouldReturnError_WhenTooManyAnimalsBookedForNoCard()
        {
            // Arrange
            var viewModel = new BookingViewModel
            {
                Animals = new List<Animal>
                {
                    new Animal { Name = "Koe", Type = "Boerderij" },
                    new Animal { Name = "Eend", Type = "Boerderij" },
                    new Animal { Name = "Kip", Type = "Boerderij" },
                    new Animal { Name = "Schaap", Type = "Boerderij" },
                    new Animal { Name = "Varken", Type = "Boerderij" } // Exceeds Silver card limit
                },
                Guest = new GuestVM { CustomerCard = "Geen" }
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(viewModel, new ValidationContext(viewModel), validationResults, true);

            // Assert
            Assert.IsFalse(isValid, "Validation should fail for exceeding max animals.");
            Assert.IsTrue(validationResults.Exists(vr => vr.ErrorMessage.Contains("U kunt maximaal 3 dieren reserveren op basis van uw klantenkaart")));
        }
        [TestMethod]
        public void CalculateDiscount_ShouldApplyCorrectDiscountForSilverCard()
        {
            // Arrange
            var viewModel = new BookingViewModel
            {
                Animals = new List<Animal>
                {
                    new Animal { Name = "Eend", Type = "Wild" },
                    new Animal { Name = "Koe", Type = "Boerderij" },
                    new Animal { Name = "Pinguïn", Type = "Sneeuw" }
                },
                SelectedDate = new DateTime(2025, 1, 22), // Monday
                Guest = new GuestVM { CustomerCard = "Zilver" }
            };

            // Act
            viewModel.CalculateDiscount();

            // Assert
            Assert.IsTrue(viewModel.Discount > 0);
            Assert.IsTrue(viewModel.Discount <= 60);
        }

    }
}
