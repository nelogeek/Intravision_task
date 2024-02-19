using Intravision_task.DTO.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Intravision_task.Interfaces
{
    public interface IMachineService
    {
        Task<IActionResult> UpdateCoinValue(int coinValue);
        Task<IActionResult> ReturnChange(CoinChangeRequest request);
        Task<IActionResult> GetCoinQuantities();
        Task<IActionResult> BuyDrink(BuyDrinkRequest request);
    }
}
