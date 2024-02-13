using Intravision_task.Models;

namespace Intravision_task.DTO
{
    public class CoinDTO
    {
        public int Id { get; set; }
        public int? Value { get; set; }
        public int? Quantity { get; set; }
        public bool IsBlocked { get; set; }

        public CoinDTO(Coin coin)
        {
            this.Id = coin.Id;
            this.Value = coin.Value;
            this.Quantity = coin.Quantity;
            this.IsBlocked = coin.IsBlocked ?? false; 
        }

        public CoinDTO()
        {

        }
    } 
}
