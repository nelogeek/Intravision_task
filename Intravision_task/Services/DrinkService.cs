using Intravision_task.Data;
using Intravision_task.DTO;
using Intravision_task.Interfaces;
using Intravision_task.Models;
using Microsoft.EntityFrameworkCore;

namespace Intravision_task.Services
{
    public class DrinkService: IDrinkService
    {
        private readonly ApplicationDbContext _context;

        public DrinkService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DrinkDTO>> GetAllDrinksAsync()
        {
            return await _context.Drinks
                .Select(d => new DrinkDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    Price = d.Price,
                    Quantity = d.Quantity,
                    ImageUrl = d.ImageUrl
                })
                .ToListAsync();
        }


        public async Task<DrinkDTO> GetDrinkByIdAsync(int id)
        {
            var drink = await _context.Drinks.FindAsync(id);
            if (drink != null)
            {
                return new DrinkDTO
                {
                    Id = drink.Id,
                    Name = drink.Name,
                    Price = drink.Price,
                    Quantity = drink.Quantity,
                    ImageUrl = drink.ImageUrl
                };
            }
            return null; //TODO or throw an exception if needed
        }


        public async Task AddDrinkAsync(DrinkDTO drinkDTO)
        {
            var drink = new Drink
            {
                Name = drinkDTO.Name,
                Price = drinkDTO.Price,
                Quantity = drinkDTO.Quantity,
                ImageUrl = drinkDTO.ImageUrl
            };
            _context.Drinks.Add(drink);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDrinkAsync(int id, DrinkDTO drinkDTO)
        {
            var drink = await _context.Drinks.FindAsync(id);
            if (drink != null)
            {
                drink.Name = drinkDTO.Name;
                drink.Price = drinkDTO.Price;
                drink.Quantity = drinkDTO.Quantity;
                drink.ImageUrl = drinkDTO.ImageUrl;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteDrinkAsync(int id)
        {
            var drinkToDelete = await _context.Drinks.FindAsync(id);
            if (drinkToDelete != null)
            {
                _context.Drinks.Remove(drinkToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> DrinkExists(int id)
        {
            return await _context.Drinks.AnyAsync(e => e.Id == id);
        }


    }
}
