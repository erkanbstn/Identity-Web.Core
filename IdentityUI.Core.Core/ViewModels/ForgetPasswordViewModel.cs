using System.ComponentModel.DataAnnotations;
namespace IdentityUI.Core.Core.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email Alanı Boş Bırakılamaz")]
        [EmailAddress(ErrorMessage = "Geçerli Bir Email Giriniz")]
        public string Email { get; set; }
    }
}
