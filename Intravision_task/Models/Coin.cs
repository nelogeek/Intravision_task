using System.ComponentModel.DataAnnotations;

namespace Intravision_task.Models
{
    public class Coin
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int? Value { get; set; }

        [Required]
        public bool? IsBlocked { get; set; }

        [Required]
        public int? Quantity { get; set; }
    }
}
