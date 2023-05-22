using Microsoft.AspNetCore.Identity;

namespace IdentityUI.Core.Localization
{
    public class LocalizationIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new()
            {
                Code = "DuplicateUserName",
                Description = $"Bu {userName} Kullanıcı Adı Farklı Bir Kullanıcı Tarafından Kullanılmaktadır."
            };
            //return base.DuplicateUserName(userName);
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return new()
            {
                Code = "DuplicateEmail",
                Description = $"Bu {email} Email Farklı Bir Kullanıcı Tarafından Kullanılmaktadır."
            };
        }
        public override IdentityError PasswordTooShort(int length)
        {
            return new()
            {
                Code = "PasswordTooShort",
                Description = $"Şifre En Az {length} Karakter Olmalıdır."
            };
        }
        public override IdentityError PasswordRequiresLower()
        {
            return new()
            {
                Code = "PasswordRequiresLower",
                Description = $"Parola Küçük Karakter İçermelidir."
            };
        }
    }
}
