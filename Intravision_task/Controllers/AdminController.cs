using Intravision_task.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Intravision_task.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly IConfiguration _configuration;

        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [ServiceFilter(typeof(SecretKeyFilter))]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CheckPassword(string secretKey)
        {
            var adminSecretKey = _configuration["AdminSecretKey"];

            if (secretKey == adminSecretKey)
            {
                return Json(new { success = true, secretKey = secretKey });
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}