using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intravision_task.Models
{
    public class Drink
    {
        public int Id { get; set; }
        
        public string? Name { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }
        
        public int? Quantity { get; set; }
        
        public string? ImageUrl { get; set; }
    
    
    }
}
