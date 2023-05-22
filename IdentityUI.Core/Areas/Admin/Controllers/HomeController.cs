using IdentityUI.Core.Areas.Admin.Models;
using IdentityUI.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityUI.Core.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public HomeController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> UserList()
        {
            var userList = await _userManager.Users.ToListAsync();
            var userViewModelList = userList.Select(b => new UserViewModel()
            {
                Email = b.Email,
                Id = b.Id,
                Name = b.UserName
            }).ToList();
            return View(userViewModelList);
        }
    }
}
