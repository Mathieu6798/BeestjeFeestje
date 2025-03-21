﻿using System;
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
            // Rule: Wild animals can't be booked with farm animals
            if (Animals.Any(a => a.Name == "Leeuw" || a.Name == "IJsbeer") &&
                Animals.Any(a => a.Type == "Boerderij"))
            {
                yield return new ValidationResult(
                    "Nom nom nom",
                    new[] { nameof(Animals) });
            }

            // Rule: Penguins can't be booked on weekends
            if (Animals.Any(a => a.Name == "Pinguïn") &&
                (SelectedDate.DayOfWeek == DayOfWeek.Saturday || SelectedDate.DayOfWeek == DayOfWeek.Sunday))
            {
                yield return new ValidationResult(
                    "Dieren in pak werken alleen doordeweeks",
                    new[] { nameof(Animals), nameof(SelectedDate) });
            }

            // Rule: Desert animals can't be booked in cold months
            if (Animals.Any(a => a.Type == "Woestijn") &&
                (SelectedDate.Month >= 10 || SelectedDate.Month <= 2))
            {
                yield return new ValidationResult(
                    "Brrrr – Veelste koud",
                    new[] { nameof(Animals), nameof(SelectedDate) });
            }

            // Rule: Snow animals can't be booked in summer months
            if (Animals.Any(a => a.Type == "Sneeuw") &&
                (SelectedDate.Month >= 6 && SelectedDate.Month <= 8))
            {
                yield return new ValidationResult(
                    "Some People Are Worth Melting For. ~ Olaf",
                    new[] { nameof(Animals), nameof(SelectedDate) });
            }

            // Rule: Maximum animals allowed for a customer card
            int maxAnimals = 10;
            if (Guest != null)
            {
                if (Guest.CustomerCard == "Zilver") maxAnimals = 4;
                else if (Guest.CustomerCard == "Goud") maxAnimals = int.MaxValue;
                else if (Guest.CustomerCard == "Platina") maxAnimals = int.MaxValue;
                else if (Guest.CustomerCard == "Geen" || Guest.CustomerCard == null) maxAnimals = 3;
                if (Guest.CustomerCard != "Platina" &&
                    Animals.Any(a => a.Type == "VIP"))
                {
                    yield return new ValidationResult(
                        "Alleen houders van een Platina-kaart kunnen VIP-dieren boeken.",
                        new[] { nameof(Animals), nameof(Guest.CustomerCard) });
                }
            }

            if (Animals.Count > maxAnimals)
            {
                yield return new ValidationResult(
                    $"U kunt maximaal {maxAnimals} dieren reserveren op basis van uw klantenkaart. ",
                    new[] { nameof(Animals), nameof(Guest.CustomerCard) });
            }
        }

        public List<string> CalculateDiscount()
        {
            int discount = 0;
            List<string> rules = new List<string>();
            // Rule: Bonus discount for 3 or more animals of the same type
            if (Animals.GroupBy(a => a.Type).Any(g => g.Count() >= 3))
            {
                Discount += 10;
                rules.Add("Bonuskorting bij 3 of meer dieren van hetzelfde type.");
            }

            // Rule: Discount for "Eend" without randomness (1-in-6 replaced with fixed logic)
            if (Animals.Any(a => a.Name == "Eend"))
            {
                Random random = new Random();
                if (random.Next(1, 7) == 1) // 1 in 6 chance
                {
                    Discount += 50;
                    rules.Add("Korting voor \"Eend\" zonder willekeur (1-op-6 vervangen door vaste logica).");
                }
            }

            // Rule: Weekday discount (Monday and Tuesday)
            if (SelectedDate.DayOfWeek == DayOfWeek.Monday || SelectedDate.DayOfWeek == DayOfWeek.Tuesday)
            {
                Discount += 15;
                rules.Add("Korting op weekdagen (maandag en dinsdag).");
            }

            // Rule: Unique letter count in animal names contributes to discount
            foreach (var animal in Animals)
            {
                Discount += animal.Name.ToUpper().Distinct().Count(c => c >= 'A' && c <= 'Z') * 2;
                rules.Add($"Unieke lettertelling in dierennamen draagt ​​bij aan korting: {animal.Name}.");
            }

            // Rule: Customer card holders get a flat discount
            if (Guest != null && !string.IsNullOrEmpty(Guest.CustomerCard))
            {
                Discount += 10;
                rules.Add("Klantenkaarthouders krijgen een vaste korting.");
            }

            // Cap the discount at 60
            Discount = Math.Min(Discount, 60);
            return rules;
        }
    }
}
