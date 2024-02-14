using Intravision_task.Models;

namespace Intravision_task.DTO
{
    public class MachineDTO
    {
        public List<CoinDTO> Coins { get; set; }
        public List<DrinkDTO> Drinks { get; set; }
        public decimal CurrentAmount { get; set; }

        public MachineDTO()
        {
            CurrentAmount = 0;
        }
    }
}
