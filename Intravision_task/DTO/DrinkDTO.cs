using Intravision_task.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intravision_task.DTO
{
    public class DrinkDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? ImageUrl { get; set; }

        public DrinkDTO(Drink drink)
        {
            this.Id = drink.Id;
            this.Name = drink.Name;
            this.Price = drink.Price;
            this.ImageUrl = drink.ImageUrl;
        }

        public DrinkDTO()
        {

        }

    }
}
