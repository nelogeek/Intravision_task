using Intravision_task.Data;
using Intravision_task.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Intravision_task.Controllers
{
    public class MachineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MachineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Machine/Index
        public async Task<IActionResult> Index()
        {
            var drinks = await _context.Drinks
                .Select(d => new DrinkDTO
                {
                    Id = d.Id,
                    Name = d.Name,
                    Price = d.Price,
                    Quantity = d.Quantity,
                    ImageUrl = d.ImageUrl
                })
                .ToListAsync();

            return View(drinks);
        }



    }
}
