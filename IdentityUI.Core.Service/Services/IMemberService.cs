using IdentityUI.Core.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityUI.Core.Service.Services
{
    public interface IMemberService
    {
        Task<UserViewModel> GetUserViewModelByUserNameAsync(string userName);
        Task<(bool, IEnumerable<IdentityError>)> ChangePasswordAsync(string userName, string oldPassword, string newPassword);
        Task<bool> CheckPasswordAsync(string userName, string oldPassword);
        Task<UserEditViewModel> GetUserEditViewModelAsync(string userName);
        Task<SelectList> GetGenderSelectListAsync();
        Task<(bool, IEnumerable<IdentityError>)> EditUserAsync(UserEditViewModel userEditViewModel, string userName);
        List<ClaimViewModel> GetClaimViewModel(ClaimsPrincipal claimsPrincipal);
        
    }
}
