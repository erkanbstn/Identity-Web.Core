using IdentityUI.Core.Models;
using IdentityUI.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityUI.Core.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public MemberController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
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
    }
}
