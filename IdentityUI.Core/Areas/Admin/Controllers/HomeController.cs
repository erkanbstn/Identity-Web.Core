using IdentityUI.Core.Core.ViewModels;
using IdentityUI.Core.Extensions;
using IdentityUI.Core.Repository.Models;
using IdentityUI.Core.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IdentityUI.Core.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }
        // Users
        public async Task<IActionResult> UserList()
        {
            return View(await _homeService.UserToListAsync());
        }
        // Roles
        public async Task<IActionResult> RoleList()
        {
            return View(await _homeService.RoleViewModelToListAsync());
        }
        public IActionResult NewRole()
        {
            return View();
        }
        [Authorize(Roles = "Role-Action")]
        [HttpPost]
        public async Task<IActionResult> NewRole(AddRoleViewModel addRoleViewMode)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _homeService.CreateRoleAsync(addRoleViewMode.Name);
            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors);
                return View();
            }
            return RedirectToAction(nameof(HomeController.RoleList));
        }
        public async Task<IActionResult> EditRole(string id)
        {
            return View(await _homeService.ByRoleViewModelToListAsync(id));
        }
        [HttpPost]
        [Authorize(Roles = "Role-Action")]
        public async Task<IActionResult> EditRole(RoleViewModel roleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _homeService.UpdateRoleAsync(roleViewModel.Id, roleViewModel.Name);
            TempData["Success"] = "Rol Başarıyla Güncellendi";
            return View();
        }
        [Authorize(Roles = "Role-Action")]
        public async Task<IActionResult> RemoveRole(string id)
        {
            await _homeService.DeleteRoleAsync(id);
            return RedirectToAction(nameof(HomeController.RoleList));
        }
        public async Task<IActionResult> AssignToRole(string id)
        {
            ViewBag.userId = id;
            return View(await _homeService.GetAssignToRoleViewModel(id));
        }
        [HttpPost]
        [Authorize(Roles = "Role-Action")]
        public async Task<IActionResult> AssignToRole(string userId, List<AssignToRoleViewModel> assignToRoleViewModel)
        {

            foreach (var item in assignToRoleViewModel)
            {
                if (item.Exist)
                {
                    await _homeService.AddToRoleAsync(item.Name, userId);
                }
                else
                {
                    await _homeService.RemoveFromRoleAsync(item.Name, userId);
                }
            }
            return RedirectToAction(nameof(HomeController.UserList));
        }

    }
}
