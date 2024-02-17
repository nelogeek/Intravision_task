using Intravision_task.Models;
using System.ComponentModel.DataAnnotations;

namespace Intravision_task.DTO
{
    public class CoinDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The value field is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0.")]
        public int? Value { get; set; }

        [Required(ErrorMessage = "The quantity field is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The quantity must be greater than 0.")]
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
