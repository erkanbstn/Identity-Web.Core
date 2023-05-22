using IdentityUI.Core.Models;
using IdentityUI.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityUI.Core.Extensions;
namespace IdentityUI.Core.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SignIn()
        {
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
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
            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, signInViewModel.Password, signInViewModel.RememberMe, true);
            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl);
            }
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string> { "3 Dakika Boyunca Giriş Yapamazsınız Lütfen Daha Sonra Tekrar Deneyin." });
                return View();
            }

            ModelState.AddModelErrorList(new List<string> { $"Email veya Parola Yanlıştır.", $"Başarısız Giriş Sayısı : { await _userManager.GetAccessFailedCountAsync(hasUser)}" });
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userManager.CreateAsync(new() { UserName = signUpViewModel.UserName, PhoneNumber = signUpViewModel.Phone, Email = signUpViewModel.Email }, signUpViewModel.PasswordConfirm);
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
        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

    }
}
