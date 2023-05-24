using IdentityUI.Core.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityUI.Core.Extensions;
using System.Security.Claims;
using IdentityUI.Core.Repository.Models;
using IdentityUI.Core.Service.Services;

namespace IdentityUI.Core.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        public IActionResult SignIn()
        {
            return View();
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel resetPasswordViewModel)
        {
            var hasUser = await _authService.SendEmailToForgetPasswordAsync(resetPasswordViewModel.Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Bu Emaile Sahip Kullanıcı Bulunamamıştır.");
                return View();
            }
            string passwordResetToken = await _authService.GeneratePasswordResetTokenAsync(hasUser);
            var passwordResetLink = Url.Action("ResetPassword", "Auth", new { userId = hasUser.Id, Token = passwordResetToken }, HttpContext.Request.Scheme);
            await _authService.SendResetPasswordEmail(passwordResetLink, hasUser.Email);
            // https://localhost:7278?userId=1&token=asdasddadasd
            TempData["success"] = "Şifre Yenileme Linki E-Posta Adresinize Gönderilmiştir.";
            return RedirectToAction("ForgetPassword");
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signInViewModel, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Action("Index", "Member");

            var hasUser = await _authService.FindByEmailAsync(signInViewModel.Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya Parola Yanlıştır.");
                return View();
            }
            //var passwordCheck = await _userManager.CheckPasswordAsync(hasUser, signInViewModel.Password);
            //if (!passwordCheck)
            //{
            //    ModelState.AddModelError(string.Empty, "Email veya Parola Yanlıştır.");
            //    return View();
            //}
            var signInResult = await _authService.PasswordSignInAsync(hasUser, signInViewModel.Password, signInViewModel.RememberMe);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string> { "3 Dakika Boyunca Giriş Yapamazsınız Lütfen Daha Sonra Tekrar Deneyin." });
                return View();
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelErrorList(new List<string> { $"Email veya Parola Yanlıştır.", $"Başarısız Giriş Sayısı : {await _authService.GetAccessFailedCountAsync(hasUser)}" });
                return View();
            }
            if (hasUser.BirthDay.HasValue)
            {
                await _authService.SignInWithClaimsAsync(hasUser, signInViewModel.RememberMe);
            }
            return Redirect(returnUrl);
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _authService.CreateAsync(signUpViewModel.UserName, signUpViewModel.Phone, signUpViewModel.Email, signUpViewModel.Password);
            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors.Select(b => b.Description).ToList());
                return View();
            }
            var user = await _authService.FindByNameAsync(signUpViewModel.UserName);
            var claimResult = await _authService.AddClaimAsync(user);
            if (!claimResult.Succeeded)
            {
                ModelState.AddModelErrorList(claimResult.Errors.Select(b => b.Description).ToList());
                return View();
            }
            if (result.Succeeded)
            {
                ViewBag.successmessage = "Üyelik Kayıt İşlemi Başarıyla Gerçekleştirildi.";
                return View();
            }
            //foreach (IdentityError item in result.Errors)
            //{
            //    ModelState.AddModelError(string.Empty, item.Description);
            //}
            ModelState.AddModelErrorList(result.Errors.Select(b => b.Description).ToList());

            return View();
        }
        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            TempData["SuccessMessage"] = null;
            TempData["userId"] = userId;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            var userId = TempData["userId"];
            var token = TempData["token"];

            if (userId == null || token == null)
            {
                throw new Exception("Bir Hata Meydana Geldi");
            }

            var hasUser = await _authService.FindByIdAsync(userId.ToString());
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı Bulunamamıştır");
                return View();
            }
            var result = await _authService.ResetPasswordAsync(hasUser, token, resetPasswordViewModel.Password);
            if (result.Succeeded)
            {
                await _authService.UpdateSecurityStampAsync(hasUser);
                TempData["SuccessMessage"] = "Şifreniz Başarıyla Yenilenmiştir";
            }
            else
            {
                ModelState.AddModelErrorList(result.Errors.Select(b => b.Description).ToList());
            }
            return View();
        }
        public async Task SignOut()
        {
            await _authService.SignOutAsync();
        }
        public IActionResult AccessDenied(string ReturnUrl)
        {
            ViewBag.message = "Bu Sayfayı Görüntülemeye Yetkiniz Yoktur.";
            return View();
        }
    }
}
