using Intravision_task.Models;

namespace Intravision_task.DTO
{
    public class MachineDTO
    {
        public IEnumerable<CoinDTO> Coins { get; set; }
        public IEnumerable<DrinkDTO> Drinks { get; set; }
        public decimal CurrentAmount { get; set; }

        public MachineDTO()
        {
            CurrentAmount = 0;
        }
    }
}
