using Intravision_task.DTO;

namespace Intravision_task.Interfaces
{
    public interface ICoinService
    {
        Task<IEnumerable<CoinDTO>> GetAllCoinsAsync();
        Task AddCoinAsync(CoinDTO coinDTO);
        Task UpdateCoinAsync(int id, CoinDTO coinDTO);
        Task DeleteCoinAsync(int id);
        Task<CoinDTO> GetCoinByIdAsync(int id);
        Task<CoinDTO> GetCoinByValueAsync(int value);
        Task<bool> CoinExistsAsync(int id);
    }
}
