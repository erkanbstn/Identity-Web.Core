using Microsoft.AspNetCore.Identity;

namespace IdentityUI.Core.Models
{
    public class AppUser : IdentityUser
    {
        public string? City { get; set; }
        public string? Picture { get; set; }
        public DateTime? BirthDay { get; set; }
        public byte? Gender { get; set; }
    }
}
