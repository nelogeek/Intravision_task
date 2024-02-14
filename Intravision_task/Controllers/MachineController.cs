using Intravision_task.Data;
using Intravision_task.DTO;
using Intravision_task.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Intravision_task.Controllers
{
    public class MachineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MachineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Machine/Index
        public async Task<IActionResult> Index()
        {

            var coins = await _context.Coins.ToListAsync();
            var drinks = await _context.Drinks.ToListAsync();

            var machineDTO = new MachineDTO
            {
                Coins = coins.OrderBy(coin => coin.Value).Select(coin => new CoinDTO
                {
                    Id = coin.Id,
                    Value = coin.Value,
                    IsBlocked = coin.IsBlocked ?? false,
                    Quantity = coin.Quantity
                }).ToList(),

                Drinks = drinks.Select(drink => new DrinkDTO
                {
                    Id = drink.Id,
                    Name = drink.Name,
                    Price = drink.Price,
                    Quantity = drink.Quantity,
                    ImageUrl = drink.ImageUrl
                }).ToList(),

                CurrentAmount = 0
            };

            return View(machineDTO);
        }

        // POST: /Machine/UpdateCoinValue
        [HttpPost]
        public ActionResult UpdateCoinValue(int coinValue)
        {
            var coin = _context.Coins.FirstOrDefault(c => c.Value == coinValue);
            if (coin != null)
            {
                coin.Quantity += 1; 
                _context.SaveChanges(); 
                return Json(new { success = true, message = "Coin value updated successfully" });
            }
            else
            {
                return Json(new { success = false, message = "Coin not found" });
            }
        }


        public class CoinChangeRequest
        {
            public int TenCoins { get; set; }
            public int FiveCoins { get; set; }
            public int TwoCoins { get; set; }
            public int OneCoins { get; set; }
        }

        // POST: /Machine/ReturnChange
        [HttpPost]
        public IActionResult ReturnChange([FromBody] CoinChangeRequest request)
        {
            try
            {
                int tenCoins = request.TenCoins;
                int fiveCoins = request.FiveCoins;
                int twoCoins = request.TwoCoins;
                int oneCoins = request.OneCoins;

                var coinsInMachine = _context.Coins.ToList();

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

                    _context.SaveChanges();

                    return Json(new { success = true, message = "Change successfully returned" });
                }
                else
                {
                    return Json(new { success = false, message = "Not enough coins in the machine" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public class BuyDrinkRequest
        {
            public int DrinkId { get; set; }
            public decimal DrinkPrice { get; set; }
            public decimal CurrentAmount { get; set; }
        }

        // POST: /Machine/BuyDrink
        [HttpPost]
        public async Task<IActionResult> BuyDrink([FromBody] BuyDrinkRequest request)
        {
            int drinkId = request.DrinkId;
            decimal drinkPrice = request.DrinkPrice;
            decimal currentAmount = request.CurrentAmount;
            if (currentAmount >= drinkPrice)
            {
                currentAmount -= drinkPrice;

                var drink = await _context.Drinks.FindAsync(drinkId);
                if (drink != null)
                {
                    drink.Quantity--;
                    _context.SaveChanges();
                }

                return Ok("Drink purchased successfully.");
            }
            else
            {
                return BadRequest("Not enough money to buy this drink!");
            }
        }



    }
}
