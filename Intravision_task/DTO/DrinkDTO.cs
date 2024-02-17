using Intravision_task.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intravision_task.DTO
{
    public class DrinkDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int? Quantity { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        //[RegularExpression(@"\b(?:https?://|www\.)\S+\b", ErrorMessage = "Invalid URL format.")]
        public string? ImageUrl { get; set; }

        public DrinkDTO(Drink drink)
        {
            this.Id = drink.Id;
            this.Name = drink.Name;
            this.Price = drink.Price;
            this.ImageUrl = drink.ImageUrl;
            this.Quantity = drink.Quantity;
        }

        public DrinkDTO()
        {

        }

    }
}
