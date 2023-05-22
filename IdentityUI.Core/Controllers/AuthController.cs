using IdentityUI.Core.Models;
using IdentityUI.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult SignIn()
        {
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(int id)
        {
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
            foreach (IdentityError item in result.Errors)
            {
                ModelState.AddModelError(string.Empty, item.Description);
            }
            return View();
        }
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn");
        }

    }
}
