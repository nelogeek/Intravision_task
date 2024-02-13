using Microsoft.AspNetCore.Mvc;

namespace Intravision_task.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
