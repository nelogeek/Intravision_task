using System.ComponentModel.DataAnnotations;

namespace Intravision_task.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password don't match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm the password")]
        public string PasswordConfirm { get; set; }
    }
}
