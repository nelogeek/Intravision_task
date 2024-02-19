using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Intravision_task.Filters
{
    public class SecretKeyFilter : IActionFilter
    {
        private readonly IConfiguration _configuration;

        public SecretKeyFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var secretKey = context.HttpContext.Request.Query["secretKey"].ToString();
            var adminSecretKey = _configuration["AdminSecretKey"];

            if (secretKey != adminSecretKey)
            {
                context.Result = new ForbidResult(); // Запрет доступа
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}
