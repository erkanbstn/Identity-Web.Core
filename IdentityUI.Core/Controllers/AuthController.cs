using IdentityUI.Core.Models;
using IdentityUI.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityUI.Core.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AuthController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
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
            return View();
        }

    }
}
