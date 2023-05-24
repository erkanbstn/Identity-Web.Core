using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityUI.Core.Core.ViewModels
{
    public class AddRoleViewModel
    {
        [Required(ErrorMessage = "Rol Alanı Boş Bırakılamaz")]
        public string Name { get; set; }
    }
}
