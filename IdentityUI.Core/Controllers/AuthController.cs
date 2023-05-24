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
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
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
            var hasUser = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Bu Emaile Sahip Kullanıcı Bulunamamıştır.");
                return View();
            }
            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);
            var passwordResetLink = Url.Action("ResetPassword", "Auth", new { userId = hasUser.Id, Token = passwordResetToken }, HttpContext.Request.Scheme);
            await _emailService.SendResetPasswordEmail(passwordResetLink, hasUser.Email);
            // https://localhost:7278?userId=1&token=asdasddadasd
            TempData["success"] = "Şifre Yenileme Linki E-Posta Adresinize Gönderilmiştir.";
            return RedirectToAction("ForgetPassword");
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signInViewModel, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Action("Index", "Member");
            var hasUser = await _userManager.FindByEmailAsync(signInViewModel.Email);

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
            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, signInViewModel.Password, signInViewModel.RememberMe, true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string> { "3 Dakika Boyunca Giriş Yapamazsınız Lütfen Daha Sonra Tekrar Deneyin." });
                return View();
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelErrorList(new List<string> { $"Email veya Parola Yanlıştır.", $"Başarısız Giriş Sayısı : {await _userManager.GetAccessFailedCountAsync(hasUser)}" });
                return View();
            }
            if (hasUser.BirthDay.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(hasUser, signInViewModel.RememberMe, new[] { new Claim("birthdate", hasUser.BirthDay.Value.ToString()) });
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
            var result = await _userManager.CreateAsync(new() { UserName = signUpViewModel.UserName, PhoneNumber = signUpViewModel.Phone, Email = signUpViewModel.Email }, signUpViewModel.PasswordConfirm);
            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors.Select(b => b.Description).ToList());
                return View();
            }
            var exchangeExpireClaim = new Claim("ExchangeExpireDate", DateTime.Now.AddDays(10).ToString());
            var user = await _userManager.FindByNameAsync(signUpViewModel.UserName);
            var claimResult = await _userManager.AddClaimAsync(user, exchangeExpireClaim);
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
            var hasUser = await _userManager.FindByIdAsync(userId);
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

            var hasUser = await _userManager.FindByIdAsync(userId.ToString());
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı Bulunamamıştır");
                return View();
            }
            var result = await _userManager.ResetPasswordAsync(hasUser, token.ToString(), resetPasswordViewModel.Password);
            if (result.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(hasUser);
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
            await _signInManager.SignOutAsync();
        }
        public IActionResult AccessDenied(string ReturnUrl)
        {
            ViewBag.message = "Bu Sayfayı Görüntülemeye Yetkiniz Yoktur.";
            return View();
        }
    }
}
