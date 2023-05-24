
using System.ComponentModel.DataAnnotations;

namespace IdentityUI.Core.Core.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Rol Alanı Boş Bırakılamaz")]
        public string Name { get; set; }
    }
}
