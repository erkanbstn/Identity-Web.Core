using IdentityUI.Core.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityUI.Core.Repository.Models
{
    public class AppUser : IdentityUser
    {
        public string? City { get; set; }
        public string? Picture { get; set; }
        public DateTime? BirthDay { get; set; }
        public Gender? Gender { get; set; }
    }
}
