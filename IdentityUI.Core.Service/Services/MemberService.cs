using IdentityUI.Core.Core.Models;
using IdentityUI.Core.Core.ViewModels;
using IdentityUI.Core.Repository.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityUI.Core.Service.Services
{
    public class MemberService : IMemberService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IFileProvider _fileProvider;

        public MemberService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IFileProvider fileProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileProvider = fileProvider;
        }

        public async Task<UserViewModel> GetUserViewModelByUserNameAsync(string userName)
        {
            var currentUser = await _userManager.FindByNameAsync(userName);
            return new UserViewModel { Email = currentUser.Email, UserName = currentUser.UserName, Phone = currentUser.PhoneNumber, Picture = currentUser.Picture };
        }

        public async Task<bool> CheckPasswordAsync(string userName, string oldPassword)
        {
            var currentUser = await _userManager.FindByNameAsync(userName);
            return await _userManager.CheckPasswordAsync(currentUser, oldPassword);
        }

        public async Task<(bool, IEnumerable<IdentityError>)> ChangePasswordAsync(string userName, string oldPassword, string newPassword)
        {
            var currentUser = await _userManager.FindByNameAsync(userName);
            var result = await _userManager.ChangePasswordAsync(currentUser, oldPassword, newPassword);
            if (!result.Succeeded)
            {
                return (false, result.Errors);
            }
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, newPassword, true, false);
            await _userManager.UpdateSecurityStampAsync(currentUser);
            return (true, null);
        }
        public async Task<UserEditViewModel> GetUserEditViewModelAsync(string userName)
        {
            var currentUser = await _userManager.FindByNameAsync(userName);
            return  new UserEditViewModel()
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                Phone = currentUser.PhoneNumber,
                Gender = currentUser.Gender,
                BirthDay = currentUser.BirthDay,
                City = currentUser.City
            };
        }
        public async Task<SelectList> GetGenderSelectListAsync()
        {
            return new SelectList(Enum.GetNames(typeof(Gender)));
        }
        public async Task<(bool, IEnumerable<IdentityError>)> EditUserAsync(UserEditViewModel userEditViewModel,string userName)
        {
            var currentUser = await _userManager.FindByNameAsync(userName);
            currentUser.UserName = userEditViewModel.UserName;
            currentUser.Email = userEditViewModel.Email;
            currentUser.BirthDay = userEditViewModel.BirthDay;
            currentUser.PhoneNumber = userEditViewModel.Phone;
            currentUser.Gender = userEditViewModel.Gender;
            currentUser.City = userEditViewModel.City;

            if (userEditViewModel.Picture != null && userEditViewModel.Picture.Length > 0)
            {
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");
                var randomFileName = $"{Guid.NewGuid()}{Path.GetExtension(userEditViewModel.Picture.FileName)}";
                var newPicturePath = Path.Combine(wwwrootFolder.First(b => b.Name == "UserPictures").PhysicalPath, randomFileName);
                using var stream = new FileStream(newPicturePath, FileMode.Create);
                await userEditViewModel.Picture.CopyToAsync(stream);
                currentUser.Picture = randomFileName;
            }
            var updateToUser = await _userManager.UpdateAsync(currentUser);
            if (!updateToUser.Succeeded)
            {
                return (false, updateToUser.Errors);
            }
            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();

            if (currentUser.BirthDay.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(currentUser, true, new[] { new Claim("birthdate", currentUser.BirthDay.Value.ToString()) });
            }
            else
            {
                await _signInManager.SignInAsync(currentUser, true);
            }
            return (true, null);

        }
        public List<ClaimViewModel> GetClaimViewModel(ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.Select(b => new ClaimViewModel()
            {
                Issuer = b.Issuer,
                Value = b.Value,
                Type = b.Type,
            }).ToList();
        }
    }
}
