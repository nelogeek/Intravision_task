using Intravision_task.Models;
using Microsoft.EntityFrameworkCore;


namespace Intravision_task.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Coin> Coins { get; set; }
        public DbSet<Drink> Drinks { get; set; }
    }
}
