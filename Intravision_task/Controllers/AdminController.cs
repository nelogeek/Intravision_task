using Microsoft.AspNetCore.Mvc;

namespace Intravision_task.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
