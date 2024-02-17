using Intravision_task.Data;
using Intravision_task.DTO;
using Intravision_task.Interfaces;
using Intravision_task.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Intravision_task.Controllers
{
    public class DrinksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDrinkService _drinkService;

        public DrinksController(ApplicationDbContext context, IDrinkService drinkService)
        {
            _context = context;
            _drinkService = drinkService;
        }

        // GET: /Drinks
        public async Task<IActionResult> Index()
        {
            IEnumerable<DrinkDTO> drinks = await _drinkService.GetAllDrinksAsync();
            return View(drinks);
        }

        // GET: /Drinks/Add
        public IActionResult Add()
        {
            return View();
        }

        // POST: /Drinks/Add
        [HttpPost]
        public async Task<IActionResult> Add(DrinkDTO drinkDTO)
        {
            if (ModelState.IsValid)
            {
                await _drinkService.AddDrinkAsync(drinkDTO);
                return RedirectToAction(nameof(Index));
            }

            return View(drinkDTO);
        }

        // GET: /Drinks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drinkDTO = await _drinkService.GetDrinkByIdAsync(id.Value);
            if (drinkDTO == null)
            {
                return NotFound();
            }

            return View(drinkDTO);
        }

        // POST: /Drinks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Quantity,ImageUrl")] DrinkDTO drinkDTO)
        {
            if (id != drinkDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _drinkService.UpdateDrinkAsync(id, drinkDTO);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Error updating drink.");
                    throw;
                }

            }

            return View(drinkDTO);
        }



        // POST: /Drinks/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _drinkService.DeleteDrinkAsync(id);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> ImportDrinks()
        {
            try
            {
                var file = Request.Form.Files[0];

                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }

                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    var jsonString = await stream.ReadToEndAsync();
                    var drinks = JsonSerializer.Deserialize<List<DrinkDTO>>(jsonString);

                    foreach (var drink in drinks)
                    {
                        await _drinkService.AddDrinkAsync(drink);
                    }

                    return Ok(new { message = "Drinks imported successfully." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error importing drinks: {ex.Message}" });
            }
        }


        private bool DrinkExists(int id)
        {
            return _context.Drinks.Any(e => e.Id == id);
        }
    }
}
