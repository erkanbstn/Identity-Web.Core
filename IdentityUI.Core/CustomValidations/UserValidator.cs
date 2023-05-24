using IdentityUI.Core.Repository.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityUI.Core.CustomValidations
{
    public class UserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            // UserName Contain Numeric In First Letter
            var errors = new List<IdentityError>();
            //string[] Digits = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            //foreach (var item in Digits)
            //{
            //    if (user.UserName[0].ToString() == item)
            //    {
            //        errors.Add(new IdentityError()
            //        {
            //            Code = "UserNameContainsFirstLetterDigit",
            //            Description = "User Name Cannot Be Contain Digit In The First Letter"
            //        });
            //    }
            //}
            var isDigit = int.TryParse(user.UserName[0]!.ToString(), out _);
            if (isDigit)
            {
                errors.Add(new() { Code = "UserNameContainsFirstLetterDigit", Description = "Kullanıcı Adı İlk Hanede Rakam İçeremez" });
            }
            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
