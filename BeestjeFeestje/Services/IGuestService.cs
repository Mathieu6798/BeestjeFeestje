using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDomain.Guest;
namespace BeestjeFeestje.Services
{
    public interface IGuestService
    {
        Task<(bool Succeeded, string Password, string ErrorMessage)> CreateGuestAsync(IGuestVM model);
        Task<List<IGuestVM>> GetAllGuestsAsync();
        Task<IGuestVM> GetGuestByIdAsync(string id);
        Task<(bool Succeeded, string ErrorMessage)> EditGuestAsync(IGuestVM model);
        Task<(bool Succeeded, string ErrorMessage)> DeleteGuestAsync(string id);
    }
}
