using Intravision_task.Data;
using Intravision_task.DTO;
using Intravision_task.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Intravision_task.Controllers
{
    public class DrinksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DrinksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Drinks
        public IActionResult Index()
        {
            var drinks = _context.Drinks
                                .Select(d => new DrinkDTO
                                {
                                    Id = d.Id,
                                    Name = d.Name,
                                    Price = d.Price,
                                    ImageUrl = d.ImageUrl
                                })
                                .ToList();
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
                var drink = new Drink
                {
                    Name = drinkDTO.Name,
                    Price = drinkDTO.Price,
                    ImageUrl = drinkDTO.ImageUrl
                };

                _context.Drinks.Add(drink);
                await _context.SaveChangesAsync();

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

            var drink = await _context.Drinks.FindAsync(id);
            if (drink == null)
            {
                return NotFound();
            }

            var drinkDTO = new DrinkDTO
            {
                Id = drink.Id,
                Name = drink.Name,
                Price = drink.Price,
                ImageUrl = drink.ImageUrl
            };

            return View(drinkDTO);
        }

        // POST: /Drinks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,ImageUrl")] DrinkDTO drinkDTO)
        {
            if (id != drinkDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var drink = await _context.Drinks.FindAsync(id);
                    if (drink == null)
                    {
                        return NotFound();
                    }

                    drink.Name = drinkDTO.Name;
                    drink.Price = drinkDTO.Price;
                    drink.ImageUrl = drinkDTO.ImageUrl;

                    _context.Update(drink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DrinkExists(drinkDTO.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(drinkDTO);
        }

        // POST: /Drinks/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var drink = await _context.Drinks.FindAsync(id);
            if (drink == null)
            {
                return NotFound();
            }

            _context.Drinks.Remove(drink);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool DrinkExists(int id)
        {
            return _context.Drinks.Any(e => e.Id == id);
        }
    }
}
