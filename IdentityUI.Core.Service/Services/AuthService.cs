using IdentityUI.Core.Core.ViewModels;
using IdentityUI.Core.Repository.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityUI.Core.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public async Task<AppUser> SendEmailToForgetPasswordAsync(string email)
        {
            var hasUser = await _userManager.FindByEmailAsync(email);
            if (hasUser == null)
            {
                return null;
            }
            return hasUser;
        }
        public async Task<string> GeneratePasswordResetTokenAsync(AppUser appUser)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(appUser);
        }
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<AppUser> FindByEmailAsync(string email)
        {
            var hasUser = await _userManager.FindByEmailAsync(email);
            if (hasUser == null)
            {
                return null;
            }
            return hasUser;
        }
        public async Task<AppUser> FindByIdAsync(string id)
        {
            var hasUser = await _userManager.FindByIdAsync(id);
            if (hasUser == null)
            {
                return null;
            }
            return hasUser;
        }
        public async Task<AppUser> FindByNameAsync(string userName)
        {
            var hasUser = await _userManager.FindByNameAsync(userName);
            if (hasUser == null)
            {
                return null;
            }
            return hasUser;
        }
        public async Task<SignInResult> PasswordSignInAsync(AppUser appUser, string password, bool rememberMe)
        {
            return await _signInManager.PasswordSignInAsync(appUser, password, rememberMe, true);
        }
        public async Task SignInWithClaimsAsync(AppUser appUser, bool rememberMe)
        {
            await _signInManager.SignInWithClaimsAsync(appUser, rememberMe, new[] { new Claim("birthdate", appUser.BirthDay.Value.ToString()) });
        }
        public async Task<IdentityResult> ResetPasswordAsync(AppUser appUser, object? token, string password)
        {
            return await _userManager.ResetPasswordAsync(appUser, token.ToString(), password);
        }
        public async Task<IdentityResult> UpdateSecurityStampAsync(AppUser appUser)
        {
            return await _userManager.UpdateSecurityStampAsync(appUser);
        }
        public async Task<IdentityResult> CreateAsync(string userName, string phone, string email, string password)
        {
            return await _userManager.CreateAsync(new() { UserName = userName, PhoneNumber = phone, Email = email }, password);
        }
        public async Task<IdentityResult> AddClaimAsync(AppUser appUser)
        {
            var exchangeExpireClaim = new Claim("ExchangeExpireDate", DateTime.Now.AddDays(10).ToString());
            return await _userManager.AddClaimAsync(appUser, exchangeExpireClaim);
        }
        public async Task<int> GetAccessFailedCountAsync(AppUser appUser)
        {
            return await _userManager.GetAccessFailedCountAsync(appUser);
        }
        public async Task SendResetPasswordEmail(string passwordResetLink, string email)
        {
            await _emailService.SendResetPasswordEmail(passwordResetLink, email);
        }
    }
}
