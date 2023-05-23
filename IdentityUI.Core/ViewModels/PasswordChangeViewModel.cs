using System.ComponentModel.DataAnnotations;

namespace IdentityUI.Core.ViewModels
{
    public class PasswordChangeViewModel
    {
        [Required(ErrorMessage = "Eski Parola Alanı Boş Bırakılamaz")]
        [MinLength(6,ErrorMessage = "Şifreniz En Az 6 Karakter Olabilir.")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Yeni Parola Alanı Boş Bırakılamaz")]
        [MinLength(6, ErrorMessage = "Şifreniz En Az 6 Karakter Olabilir.")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Parola Tekrar Alanı Boş Bırakılamaz")]
        [MinLength(6, ErrorMessage = "Şifreniz En Az 6 Karakter Olabilir.")]
        [Compare(nameof(NewPassword), ErrorMessage = "Şifreler Aynı Olmalıdır")]
        public string ConfirmPassword { get; set; }
    }
}
