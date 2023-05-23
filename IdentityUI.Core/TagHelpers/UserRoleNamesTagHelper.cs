using IdentityUI.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace IdentityUI.Core.TagHelpers
{
    public class UserRoleNamesTagHelper : TagHelper
    {
        private readonly UserManager<AppUser> _userManager;

        public UserRoleNamesTagHelper(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public string UserId { get; set; }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            var userRoles = await _userManager.GetRolesAsync(user);
            var stringBuilder = new StringBuilder();
            userRoles.ToList().ForEach(x =>
            {
                stringBuilder.Append(@$"<span class=""badge-bg secondary mx-2 bg-info text-black""  >{x.ToUpper()}</span>    ");
            });
            output.Content.SetHtmlContent(stringBuilder.ToString());
        }

    }
}
