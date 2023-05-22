using IdentityUI.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityUI.Core.CustomValidations
{
    public class PasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
        {
            var errors = new List<IdentityError>();
            // Password Does Not Contain UserName
            if (password!.ToLower().Contains(user.UserName!.ToLower()))
            {
                errors.Add(new() { Code = "PasswordContainUserName", Description = "Şifre Alanı Kullanıcı Adı İçeremez." });
            }
            // Password Does Not Contain 1234
            if (password!.ToLower().StartsWith("1234"))
            {
                errors.Add(new() { Code = "PasswordContain1234", Description = "Şifre Alanı 1234 İçeremez." });
            }
            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
