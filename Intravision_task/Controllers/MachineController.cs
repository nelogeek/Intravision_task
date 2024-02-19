using Intravision_task.Data;
using Intravision_task.DTO;
using Intravision_task.DTO.Requests;
using Intravision_task.Interfaces;
using Intravision_task.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Intravision_task.Controllers
{
    public class MachineController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMachineService _machineService;
        private readonly ICoinService _coinService;
        private readonly IDrinkService _drinkService;

        public MachineController(ApplicationDbContext context, IMachineService machineService, ICoinService coinService, IDrinkService drinkService)
        {
            _context = context;
            _machineService = machineService;
            _coinService = coinService;
            _drinkService = drinkService;
        }

        // GET: /Machine/Index
        public async Task<IActionResult> Index()
        {
            var coins = await _coinService.GetAllCoinsAsync();
            var drinks = await _drinkService.GetAllDrinksAsync();

            var machineDTO = new MachineDTO
            {
                Coins = coins,
                Drinks = drinks,
                CurrentAmount = 0
            };

            return View(machineDTO);
        }

        // POST: /Machine/UpdateCoinValue
        [HttpPost]
        public async Task<IActionResult> UpdateCoinValue(int coinValue)
        {
            return await _machineService.UpdateCoinValue(coinValue);
        }

        // POST: /Machine/ReturnChange
        [HttpPost]
        public async Task<IActionResult> ReturnChange([FromBody] CoinChangeRequest request)
        {
            return await _machineService.ReturnChange(request);
        }

        // GET: /Machine/GetCoinQuantities
        [HttpGet]
        public async Task<IActionResult> GetCoinQuantities()
        {
            return await _machineService.GetCoinQuantities();
        }


        // POST: /Machine/BuyDrink
        [HttpPost]
        public async Task<IActionResult> BuyDrink([FromBody] BuyDrinkRequest request)
        {
            return await _machineService.BuyDrink(request);
        }








    }
}
