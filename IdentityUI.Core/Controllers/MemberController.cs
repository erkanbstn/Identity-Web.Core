using IdentityUI.Core.Core.ViewModels;
using IdentityUI.Core.Extensions;
using IdentityUI.Core.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using IdentityUI.Core.Service.Services;

namespace IdentityUI.Core.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        private string userName => User.Identity.Name;
        public async Task<IActionResult> Index()
        {
            return View(await _memberService.GetUserViewModelByUserNameAsync(userName));
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
            if (!await _memberService.CheckPasswordAsync(userName, passwordChangeViewModel.OldPassword))
            {
                ModelState.AddModelError(string.Empty, "Eski Şifreniz Yanlıştır.");
                return View();
            }
            var (isSuccess, errors) = await _memberService.ChangePasswordAsync(userName, passwordChangeViewModel.OldPassword, passwordChangeViewModel.NewPassword);
            if (!isSuccess)
            {
                ModelState.AddModelErrorList(errors);
                return View();
            }
            TempData["Success"] = "Parolanız Başarıyla Değiştirildi";
            return RedirectToAction("PasswordChange");
        }
        public async Task<IActionResult> UserEdit()
        {
            ViewBag.genderlist = _memberService.GetGenderSelectListAsync();
            return View(await _memberService.GetUserEditViewModelAsync(userName));
        }
        [HttpPost]
        [Authorize(Policy = "İstanbulPolicy")]
        public async Task<IActionResult> UserEdit(UserEditViewModel userEditViewModel)
        {
            ViewBag.genderlist = _memberService.GetGenderSelectListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            var (isSucess,errors) = await _memberService.EditUserAsync(userEditViewModel, userName);
            if (!isSucess)
            {
                ModelState.AddModelErrorList(errors);
                return View();
            }
            TempData["Success2"] = "Üye Bilgileri Başarıyla Değiştirildi";
            return View(await _memberService.GetUserEditViewModelAsync(userName));
        }
        public IActionResult UserClaims()
        {
            return View(_memberService.GetClaimViewModel(User));
        }
    }
}
