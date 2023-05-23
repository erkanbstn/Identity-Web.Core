using IdentityUI.Core.Extensions;
using IdentityUI.Core.Models;
using IdentityUI.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityUI.Core.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public MemberController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var userViewModel = new UserViewModel { Email = currentUser.Email, UserName = currentUser.UserName, Phone = currentUser.PhoneNumber };
            return View(userViewModel);
        }
        public IActionResult ReturnTest()
        {
            return View();
        }
        public IActionResult PasswordChange()
        {
            return View();
        }
        [HttpPost]
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
            var result = await _userManager.ChangePasswordAsync(currentUser, passwordChangeViewModel.OldPassword,passwordChangeViewModel.NewPassword);
            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors.Select(b=>b.Description).ToList());
                return View();
            }
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser,passwordChangeViewModel.NewPassword,true,false);
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
                Gender=currentUser.Gender,
                BirthDay=currentUser.BirthDay,
                City=currentUser.City
            };
            return View(userEditViewModel);
        }
    }
}
