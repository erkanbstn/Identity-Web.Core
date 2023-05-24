using System.ComponentModel.DataAnnotations;

namespace IdentityUI.Core.Core.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Parola Boş Alanı Bırakılamaz")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Parola Tekrar Alanı Boş Bırakılamaz")]
        [Compare(nameof(Password), ErrorMessage = "Şifreler Aynı Olmalıdır")]
        public string PasswordConfirm { get; set; }
    }
}
