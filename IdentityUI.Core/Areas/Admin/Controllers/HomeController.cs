using IdentityUI.Core.Areas.Admin.Models;
using IdentityUI.Core.Extensions;
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
        private readonly RoleManager<AppRole> _roleManager;
        public HomeController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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
        public async Task<IActionResult> RoleList()
        {
            var roles = await _roleManager.Roles.Select(b => new RoleViewModel()
            {
                Id = b.Id,
                Name = b.Name
            }).ToListAsync();
            return View(roles);
        }
        public IActionResult NewRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NewRole(RoleViewModel roleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _roleManager.CreateAsync(new AppRole()
            {
                Name = roleViewModel.Name,
            });
            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors);
                return View();
            }
            return RedirectToAction(nameof(HomeController.RoleList));
        }
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            return View(new RoleViewModel() { Id = role.Id, Name = role.Name });
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(RoleViewModel roleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var role = await _roleManager.FindByIdAsync(roleViewModel.Id);
            role.Name=roleViewModel.Name;
            await _roleManager.UpdateAsync(role);
            TempData["Success"] = "Rol Başarıyla Güncellendi";
            return View();
        }
        public async Task<IActionResult> RemoveRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            await _roleManager.DeleteAsync(role);
            return RedirectToAction(nameof(HomeController.RoleList));
        }
    }
}
