using Intravision_task.Data;
using Intravision_task.DTO.Requests;
using Intravision_task.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Intravision_task.Services
{
    public class MachineService : IMachineService
    {
        private readonly ApplicationDbContext _context;

        public MachineService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> UpdateCoinValue(int coinValue)
        {
            var coin = _context.Coins.FirstOrDefault(c => c.Value == coinValue);
            if (coin != null)
            {
                coin.Quantity += 1;
                await _context.SaveChangesAsync();
                return new JsonResult(new { success = true, message = "Coin value updated successfully" });
            }
            else
            {
                return new JsonResult(new { success = false, message = "Coin not found" });
            }
        }

        public async Task<IActionResult> ReturnChange(CoinChangeRequest request)
        {
            try
            {
                int tenCoins = request.TenCoins;
                int fiveCoins = request.FiveCoins;
                int twoCoins = request.TwoCoins;
                int oneCoins = request.OneCoins;

                var coinsInMachine = await _context.Coins.ToListAsync();

                var tenCoin = coinsInMachine.FirstOrDefault(c => c.Value == 10);
                var fiveCoin = coinsInMachine.FirstOrDefault(c => c.Value == 5);
                var twoCoin = coinsInMachine.FirstOrDefault(c => c.Value == 2);
                var oneCoin = coinsInMachine.FirstOrDefault(c => c.Value == 1);

                if (tenCoin.Quantity >= tenCoins && fiveCoin.Quantity >= fiveCoins && twoCoin.Quantity >= twoCoins && oneCoin.Quantity >= oneCoins)
                {
                    tenCoin.Quantity -= tenCoins;
                    fiveCoin.Quantity -= fiveCoins;
                    twoCoin.Quantity -= twoCoins;
                    oneCoin.Quantity -= oneCoins;

                    await _context.SaveChangesAsync();

                    return new JsonResult(new { success = true, message = "Change successfully returned" });
                }
                else
                {
                    return new JsonResult(new { success = false, message = "Not enough coins in the machine" });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> GetCoinQuantities()
        {
            try
            {
                var coinQuantities = new
                {
                    tenCoins = (await _context.Coins.FirstOrDefaultAsync(c => c.Value == 10))?.Quantity ?? 0,
                    fiveCoins = (await _context.Coins.FirstOrDefaultAsync(c => c.Value == 5))?.Quantity ?? 0,
                    twoCoins = (await _context.Coins.FirstOrDefaultAsync(c => c.Value == 2))?.Quantity ?? 0,
                    oneCoins = (await _context.Coins.FirstOrDefaultAsync(c => c.Value == 1))?.Quantity ?? 0
                };

                return new JsonResult(new { success = true, coinQuantities });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> BuyDrink(BuyDrinkRequest request)
        {
            try
            {
                int drinkId = request.DrinkId;
                int drinkPrice = Convert.ToInt32(request.DrinkPrice);
                int currentAmount = Convert.ToInt32(request.CurrentAmount);

                var drink = await _context.Drinks.FindAsync(drinkId);

                if (drink != null && drink.Quantity > 0 && currentAmount >= drinkPrice)
                {
                    // Сумма сдачи
                    var changeAmount = currentAmount - drinkPrice;
                    int amount = changeAmount;

                    // Получаем доступные монеты для сдачи и включаем режим сложных расчетов
                    var coinsInMachine = _context.Coins.ToList();

                    int tenCoinsCount = coinsInMachine.Where(c => c.Value == 10).Sum(c => c.Quantity).GetValueOrDefault();
                    int fiveCoinsCount = coinsInMachine.Where(c => c.Value == 5).Sum(c => c.Quantity).GetValueOrDefault();
                    int twoCoinsCount = coinsInMachine.Where(c => c.Value == 2).Sum(c => c.Quantity).GetValueOrDefault();
                    int oneCoinsCount = coinsInMachine.Where(c => c.Value == 1).Sum(c => c.Quantity).GetValueOrDefault();

                    int tenCoins = Math.Min(changeAmount / 10, tenCoinsCount);
                    changeAmount -= tenCoins * 10;
                    int fiveCoins = Math.Min(changeAmount / 5, fiveCoinsCount);
                    changeAmount -= fiveCoins * 5;
                    int twoCoins = Math.Min(changeAmount / 2, twoCoinsCount);
                    changeAmount -= twoCoins * 2;
                    int oneCoins = Math.Min(changeAmount, oneCoinsCount);



                    // Проверяем, достаточно ли монет для сдачи
                    if (amount == tenCoins * 10 + fiveCoins * 5 + twoCoins * 2 + oneCoins)
                    {
                        currentAmount -= drinkPrice;
                        drink.Quantity--;

                        _context.SaveChanges();

                        return new JsonResult(new { success = true, message = "Drink purchased successfully.", currentAmount = currentAmount });
                    }
                    else
                    {
                        return new JsonResult(new { success = false, message = "Not enough coins to give change." });
                    }
                }
                else if (drink != null && drink.Quantity <= 0)
                {
                    return new JsonResult(new { success = false, message = "The drink is out of stock." });
                }
                else
                {
                    return new JsonResult(new { success = false, message = "Not enough money to buy this drink!" });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

    }
}
