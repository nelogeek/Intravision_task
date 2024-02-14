using Intravision_task.Data;
using Intravision_task.Models;
using Intravision_task.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Intravision_task.Controllers
{
    public class CoinsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoinsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Coins
        public async Task<IActionResult> Index()
        {
            var coins = await _context.Coins
                .Select(c => new CoinDTO
                {
                    Id = c.Id,
                    Value = c.Value,
                    Quantity = c.Quantity,
                    IsBlocked = c.IsBlocked ?? false
                })
                .ToListAsync();

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
            var existingCoin = await _context.Coins.FirstOrDefaultAsync(c => c.Value == coinDTO.Value);

            if (existingCoin != null)
            {
                ModelState.AddModelError("Value", "A coin with this value already exists.");
                return View(coinDTO);
            }

            if (ModelState.IsValid)
            {
                var coin = new Coin
                {
                    Value = coinDTO.Value,
                    Quantity = coinDTO.Quantity,
                    IsBlocked = coinDTO.IsBlocked
                };

                _context.Coins.Add(coin);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(coinDTO);
        }

        // GET: /Coins/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coin = _context.Coins.Find(id);
            if (coin == null)
            {
                return NotFound();
            }

            var coinDTO = new CoinDTO(coin);

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

            var existingCoin = await _context.Coins.FirstOrDefaultAsync(c => c.Value == coinDTO.Value && c.Id != id);

            if (existingCoin != null)
            {
                ModelState.AddModelError("Value", "A coin with this value already exists.");
                return View(coinDTO);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var coin = new Coin
                    {
                        Id = coinDTO.Id,
                        Value = coinDTO.Value,
                        Quantity = coinDTO.Quantity,
                        IsBlocked = coinDTO.IsBlocked
                    };

                    _context.Update(coin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoinExists(coinDTO.Id))
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
            return View(coinDTO);
        }


        // POST: /Coins/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var coin = await _context.Coins.FindAsync(id);
            if (coin == null)
            {
                return NotFound();
            }

            _context.Coins.Remove(coin);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool CoinExists(int id)
        {
            return _context.Coins.Any(e => e.Id == id);
        }
    }
}
