using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MyDomain;
using MyDomain.Guest;

namespace BeestjeFeestje.Models
{
    public class BookingViewModel : IValidatableObject
    {
        public DateTime SelectedDate { get; set; }
        public List<Animal> Animals { get; set; }
        public GuestVM Guest { get; set; }
        public int Discount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Animals.Any(a => a.Name == "Leeuw" || a.Name == "IJsbeer") &&
                Animals.Any(a => a.Type == "Boerderij"))
            {
                yield return new ValidationResult(
                    "Nom nom nom",
                    new[] { nameof(Animals) });
            }

            if (Animals.Any(a => a.Name == "Pinguïn") &&
                (SelectedDate.DayOfWeek == DayOfWeek.Saturday || SelectedDate.DayOfWeek == DayOfWeek.Sunday))
            {
                yield return new ValidationResult(
                    "Dieren in pak werken alleen doordeweeks",
                    new[] { nameof(Animals), nameof(SelectedDate) });
            }

            if (Animals.Any(a => a.Type == "Woestijn") &&
                (SelectedDate.Month >= 10 || SelectedDate.Month <= 2))
            {
                yield return new ValidationResult(
                    "Brrrr – Veelste koud",
                    new[] { nameof(Animals), nameof(SelectedDate) });
            }

            if (Animals.Any(a => a.Type == "Sneeuw") &&
                (SelectedDate.Month >= 6 && SelectedDate.Month <= 8))
            {
                yield return new ValidationResult(
                    "Some People Are Worth Melting For. ~ Olaf",
                    new[] { nameof(Animals), nameof(SelectedDate) });
            }

            int maxAnimals = 3; 
            if(Guest != null)
            {
                if (Guest.CustomerCard == "Zilver") maxAnimals = 4;
                else if (Guest.CustomerCard == "Goud") maxAnimals = int.MaxValue;
                else if (Guest.CustomerCard == "Platina") maxAnimals = int.MaxValue; 
                if (Guest.CustomerCard != "Platina" &&
                    Animals.Any(a => a.Type == "VIP"))
                {
                    yield return new ValidationResult(
                        "Only Platina card holders can book VIP animals.",
                        new[] { nameof(Animals), nameof(Guest.CustomerCard) });
                }
            }

            if (Animals.Count > maxAnimals)
            {
                yield return new ValidationResult(
                    $"You can book a maximum of {maxAnimals} animals based on your customer card.",
                    new[] { nameof(Animals), nameof(Guest.CustomerCard) });
            }
        }
        public void CalculateDiscount()
        {
            int discount = 0;

            if (Animals.GroupBy(a => a.Type).Any(g => g.Count() >= 3))
            {
                Discount += 10;
            }

            if (Animals.Any(a => a.Name == "Eend"))
            {
                Random random = new Random();
                if (random.Next(1, 7) == 1) // 1 in 6 chance
                {
                    Discount += 50;
                }
            }

            if (SelectedDate.DayOfWeek == DayOfWeek.Monday || SelectedDate.DayOfWeek == DayOfWeek.Tuesday)
            {
                Discount += 15;
            }

            foreach (var animal in Animals)
            {
                Discount += animal.Name.ToUpper().Distinct().Count(c => c >= 'A' && c <= 'Z') * 2;
            }

            if (Guest != null && !string.IsNullOrEmpty(Guest.CustomerCard))
            {
                Discount += 10;
            }

            Discount = Math.Min(Discount, 60);
        }
    }
}
