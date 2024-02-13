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
        public IActionResult Index()
        {
            List<CoinDTO> coins = _context.Coins.Select(c => new CoinDTO
            {
                Id = c.Id,
                Value = c.Value,
                Quantity = c.Quantity,
                IsBlocked = c.IsBlocked ?? false
            }).ToList();

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
                // Создание нового экземпляра монеты из DTO
                var coin = new Coin
                {
                    Value = coinDTO.Value,
                    Quantity = coinDTO.Quantity,
                    IsBlocked = coinDTO.IsBlocked
                };

                // Добавление монеты в контекст данных
                _context.Coins.Add(coin);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index)); // Перенаправляем на страницу со списком монет
            }

            return View(coinDTO); // Возвращаем обратно на страницу добавления с ошибками валидации, если они есть
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
