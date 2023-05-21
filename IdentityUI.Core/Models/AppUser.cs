using Microsoft.AspNetCore.Identity;

namespace IdentityUI.Core.Models
{
    public class AppUser : IdentityUser
    {
        public string? City { get; set; }
    }
}
