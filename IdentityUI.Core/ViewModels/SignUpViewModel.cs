using System.ComponentModel.DataAnnotations;

namespace IdentityUI.Core.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Kullanıcı Adı Alanı Boş Bırakılamaz")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email Alanı Boş Bırakılamaz")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Telefon Alanı Boş Bırakılamaz")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Parola Alanı Boş Bırakılamaz")]
        public string Password { get; set; }
        [Compare(nameof(Password), ErrorMessage = "Şifreler Aynı Olmalıdır")]
        public string PasswordConfirm { get; set; }
    }
}
