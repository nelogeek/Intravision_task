using Microsoft.AspNetCore.Identity;

namespace Intravision_task.Models
{
    public class User: IdentityUser
    {
        public string Login {  get; set; }
    }
}
