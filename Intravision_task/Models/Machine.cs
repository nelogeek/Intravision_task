namespace Intravision_task.Models
{
    public class Machine
    {
        public List<Coin> Coins { get; set; }
        public List<Drink> Drinks { get; set; }
        public decimal CurrentAmount { get; set; }
    }
}
