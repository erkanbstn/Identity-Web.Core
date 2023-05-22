using System.ComponentModel.DataAnnotations;

namespace IdentityUI.Core.ViewModels
{
    public class SignInViewModel
    {
        [Required(ErrorMessage = "Email Alanı Boş Bırakılamaz")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Parola Alanı Boş Bırakılamaz")]
        public string Password { get; set; }
        public bool RememberMe{ get; set; }
    }
}
