using Intravision_task.Data;
using Intravision_task.DTO;
using Intravision_task.Interfaces;
using Intravision_task.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intravision_task.Services
{
    public class CoinService : ICoinService
    {
        private readonly ApplicationDbContext _context;

        public CoinService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CoinDTO>> GetAllCoinsAsync()
        {
            return await _context.Coins
                .Select(c => new CoinDTO
                {
                    Id = c.Id,
                    Value = c.Value,
                    Quantity = c.Quantity,
                    IsBlocked = c.IsBlocked ?? false
                })
                .ToListAsync();
        }

        public async Task AddCoinAsync(CoinDTO coinDTO)
        {
            var coin = new Coin
            {
                Value = coinDTO.Value,
                Quantity = coinDTO.Quantity,
                IsBlocked = coinDTO.IsBlocked
            };

            _context.Coins.Add(coin);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCoinAsync(int id, CoinDTO coinDTO)
        {
            var coin = await _context.Coins.FindAsync(id);
            if (coin == null)
            {
                throw new KeyNotFoundException($"Coin with id={id} not found.");
            }

            coin.Value = coinDTO.Value;
            coin.Quantity = coinDTO.Quantity;
            coin.IsBlocked = coinDTO.IsBlocked;

            _context.Coins.Update(coin);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCoinAsync(int id)
        {
            var coin = await _context.Coins.FindAsync(id);
            if (coin == null)
            {
                throw new KeyNotFoundException($"Coin with id={id} not found.");
            }

            _context.Coins.Remove(coin);
            await _context.SaveChangesAsync();
        }

        public async Task<CoinDTO> GetCoinByIdAsync(int id)
        {
            var coin = await _context.Coins.FindAsync(id);
            return coin != null ? new CoinDTO(coin) : null;
        }

        public async Task<CoinDTO> GetCoinByValueAsync(int value)
        {
            var coin = await _context.Coins.FirstOrDefaultAsync(c => c.Value == value);
            return coin != null ? new CoinDTO(coin) : null;
        }

        public async Task<bool> CoinExistsAsync(int id)
        {
            return await _context.Coins.AnyAsync(e => e.Id == id);
        }
    }
}
