using IdentityUI.Core.Core.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace IdentityUI.Core.Core.ViewModels
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "Kullanıcı Adı Alanı Boş Bırakılamaz")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email Alanı Boş Bırakılamaz")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Telefon Alanı Boş Bırakılamaz")]
        public string Phone { get; set; }
        public string? City { get; set; }
        public IFormFile? Picture { get; set; }
        public DateTime? BirthDay { get; set; }
        public Gender? Gender { get; set; }
    }
}
