using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeestjeFeestje.DbAccess;
using MyDomain;

namespace BeestjeFeestje.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly MyAnimalDbContext _context;

        public AnimalService(MyAnimalDbContext context)
        {
            _context = context;
        }

        public async Task<List<Animal>> GetAllAnimalsAsync()
        {
            return _context.Animals.ToList();
        }

        public async Task<Animal?> GetAnimalByIdAsync(int id)
        {
            return _context.Animals.FirstOrDefault(a => a.Id == id);
        }

        public async Task<bool> CreateAnimalAsync(Animal animal)
        {
            _context.Animals.Add(animal);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> UpdateAnimalAsync(Animal animal)
        {
            var existingAnimal = _context.Animals.FirstOrDefault(a => a.Id == animal.Id);
            if (existingAnimal == null)
            {
                return false;
            }

            existingAnimal.Name = animal.Name;
            existingAnimal.Type = animal.Type;
            existingAnimal.Price = animal.Price;
            existingAnimal.Image = animal.Image;

            _context.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteAnimalAsync(int id)
        {
            var animal = _context.Animals.FirstOrDefault(a => a.Id == id);
            if (animal == null)
            {
                return false;
            }

            _context.Animals.Remove(animal);
            _context.SaveChanges();
            return true;
        }
    }
}
