namespace Intravision_task.DTO.Requests
{
    public class BuyDrinkRequest
    {
        public int DrinkId { get; set; }
        public decimal DrinkPrice { get; set; }
        public decimal CurrentAmount { get; set; }
    }
}
