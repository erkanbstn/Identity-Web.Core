using System.ComponentModel.DataAnnotations;

namespace IdentityUI.Core.Areas.Admin.Models
{
    public class AddRoleViewMode
    {
        [Required(ErrorMessage ="Rol Alanı Boş Bırakılamaz")]
        public string Name { get; set; }
    }
}
