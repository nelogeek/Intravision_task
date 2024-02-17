using Intravision_task.DTO;
using Intravision_task.Models;

namespace Intravision_task.Interfaces
{
    public interface IDrinkService
    {
        Task<IEnumerable<DrinkDTO>> GetAllDrinksAsync();
        Task<DrinkDTO> GetDrinkByIdAsync(int id);
        Task AddDrinkAsync(DrinkDTO drink);
        Task UpdateDrinkAsync(int id, DrinkDTO drink);
        Task DeleteDrinkAsync(int id);
        Task<bool> DrinkExists(int id);
    }
}
