using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDomain;
namespace BeestjeFeestje.Services
{
    public interface IAnimalService
    {
        Task<List<Animal>> GetAllAnimalsAsync();
        Task<Animal?> GetAnimalByIdAsync(int id);
        Task<bool> CreateAnimalAsync(Animal animal);
        Task<bool> UpdateAnimalAsync(Animal animal);
        Task<bool> DeleteAnimalAsync(int id);
    }
}
