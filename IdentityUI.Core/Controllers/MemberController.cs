using IdentityUI.Core.Extensions;
using IdentityUI.Core.Models;
using IdentityUI.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityUI.Core.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IFileProvider _fileProvider;

        public MemberController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IFileProvider fileProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileProvider = fileProvider;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var userViewModel = new UserViewModel { Email = currentUser.Email, UserName = currentUser.UserName, Phone = currentUser.PhoneNumber, Picture = currentUser.Picture };
            return View(userViewModel);
        }
        [Authorize(Policy = "ExchangePolicy")]
        public IActionResult ReturnTest()
        {
            return View();
        }
        public IActionResult PasswordChange()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Policy = "ViolencePolicy")]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel passwordChangeViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser, passwordChangeViewModel.OldPassword);
            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski Şifreniz Yanlıştır.");
                return View();
            }
            var result = await _userManager.ChangePasswordAsync(currentUser, passwordChangeViewModel.OldPassword, passwordChangeViewModel.NewPassword);
            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors.Select(b => b.Description).ToList());
                return View();
            }
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, passwordChangeViewModel.NewPassword, true, false);
            await _userManager.UpdateSecurityStampAsync(currentUser);
            TempData["Success"] = "Parolanız Başarıyla Değiştirildi";
            return RedirectToAction("PasswordChange");
        }
        public async Task<IActionResult> UserEdit()
        {
            ViewBag.genderlist = new SelectList(Enum.GetNames(typeof(Gender)));
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                Phone = currentUser.PhoneNumber,
                Gender = currentUser.Gender,
                BirthDay = currentUser.BirthDay,
                City = currentUser.City
            };
            return View(userEditViewModel);
        }
        [HttpPost]
        [Authorize(Policy = "İstanbulPolicy")]
        public async Task<IActionResult> UserEdit(UserEditViewModel userEditViewModel)
        {
            ViewBag.genderlist = new SelectList(Enum.GetNames(typeof(Gender)));
            if (!ModelState.IsValid)
            {
                return View();
            }
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
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
                ModelState.AddModelErrorList(updateToUser.Errors);
                return View();
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
            TempData["Success2"] = "Üye Bilgileri Başarıyla Değiştirildi";
            return View(userEditViewModel);
        }
        public IActionResult UserClaims()
        {
            var userClaim = User.Claims.Select(b => new ClaimViewModel()
            {
                Issuer = b.Issuer,
                Value = b.Value,
                Type = b.Type,
            }).ToList();
            return View(userClaim);
        }
    }
}
