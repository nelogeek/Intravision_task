using Intravision_task.Data;
using Intravision_task.Models;
using Intravision_task.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Intravision_task.Interfaces;

namespace Intravision_task.Controllers
{

    //TODO протестировать методы
    public class CoinsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICoinService _coinService;

        public CoinsController(ApplicationDbContext context, ICoinService coinService)
        {
            _context = context;
            _coinService = coinService;
        }

        // GET: /Coins
        public async Task<IActionResult> Index()
        {
            var coins = await _coinService.GetAllCoinsAsync();
            return View(coins);
        }

        // GET: /Coins/Add
        public IActionResult Add()
        {
            return View();
        }

        // POST: /Coins/Add
        [HttpPost]
        public async Task<IActionResult> Add(CoinDTO coinDTO)
        {
            if (ModelState.IsValid)
            {
                if (coinDTO.Value.HasValue)
                {
                    var existingCoin = await _coinService.GetCoinByValueAsync(coinDTO.Value.Value);
                    if (existingCoin != null)
                    {
                        ModelState.AddModelError("Value", "A coin with this value already exists.");
                        return View(coinDTO);
                    }

                    await _coinService.AddCoinAsync(coinDTO);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Value", "Value field is required.");
                    return View(coinDTO);
                }
            }

            return View(coinDTO);
        }


        // GET: /Coins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coinDTO = await _coinService.GetCoinByIdAsync(id.Value);
            if (coinDTO == null)
            {
                return NotFound();
            }

            return View(coinDTO);
        }

        // POST: /Coins/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, CoinDTO coinDTO)
        {
            if (id != coinDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _coinService.UpdateCoinAsync(id, coinDTO);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _coinService.CoinExistsAsync(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(coinDTO);
        }


        // POST: /Coins/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _coinService.DeleteCoinAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
