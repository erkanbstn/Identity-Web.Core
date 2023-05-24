using IdentityUI.Core.Repository.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityUI.Core.Service.Services
{
    public interface IAuthService
    {
        Task<AppUser> SendEmailToForgetPasswordAsync(string email);
        Task<string> GeneratePasswordResetTokenAsync(AppUser appUser);
        Task SignOutAsync();
        Task<AppUser> FindByEmailAsync(string email);
        Task<SignInResult> PasswordSignInAsync(AppUser appUser, string password, bool rememberMe);
        Task SignInWithClaimsAsync(AppUser appUser, bool rememberMe);
        Task<AppUser> FindByIdAsync(string id);
        Task<IdentityResult> ResetPasswordAsync(AppUser appUser, object? token, string password);
        Task<IdentityResult> UpdateSecurityStampAsync(AppUser appUser);
        Task<IdentityResult> CreateAsync(string userName, string phone, string email, string password);
        Task<AppUser> FindByNameAsync(string userName);
        Task<IdentityResult> AddClaimAsync(AppUser appUser);
        Task<int> GetAccessFailedCountAsync(AppUser appUser);
        Task SendResetPasswordEmail(string passwordResetLink, string email);
    }
}
