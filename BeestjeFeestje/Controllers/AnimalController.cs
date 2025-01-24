using BeestjeFeestje.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyDomain;

namespace BeestjeFeestje.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AnimalController : Controller
    {
        private readonly IAnimalService _animalService;

        public AnimalController(IAnimalService animalService)
        {
            _animalService = animalService;
        }

        public async Task<IActionResult> Index()
        {
            var animals = await _animalService.GetAllAnimalsAsync();
            return View(animals);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Animal model)
        {
            var success = await _animalService.CreateAnimalAsync(model);
            if (success)
            {
                TempData["SuccessMessage"] = "Animal added successfully!";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Failed to create the animal.";
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var animal = await _animalService.GetAnimalByIdAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            return View(animal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Animal model)
        {
            var success = await _animalService.UpdateAnimalAsync(model);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Failed to update the animal.";
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var animal = await _animalService.GetAnimalByIdAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            return View(animal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _animalService.DeleteAnimalAsync(id);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Failed to delete the animal.";
            return RedirectToAction(nameof(Delete), new { id });
        }

        public async Task<IActionResult> Details(int id)
        {
            var animal = await _animalService.GetAnimalByIdAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            return View(animal);
        }
    }
}
