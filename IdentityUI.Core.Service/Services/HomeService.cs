using IdentityUI.Core.Core.ViewModels;
using IdentityUI.Core.Repository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IdentityUI.Core.Service.Services
{
    public class HomeService : IHomeService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public HomeService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<UserAdminViewModel>> UserToListAsync()
        {
            var userList = await _userManager.Users.ToListAsync();
            return userList.Select(b => new UserAdminViewModel()
            {
                Email = b.Email,
                Id = b.Id,
                Name = b.UserName
            }).ToList();
        }
        public async Task<List<RoleViewModel>> RoleViewModelToListAsync()
        {
            return await _roleManager.Roles.Select(b => new RoleViewModel()
            {
                Id = b.Id,
                Name = b.Name
            }).ToListAsync();
        }
        public async Task<List<RoleViewModel>> ByRoleViewModelToListAsync(string id)
        {
            var role = await RoleFindByIdAsync(id);
            return await _roleManager.Roles.Select(b => new RoleViewModel()
            {
                Id = b.Id,
                Name = b.Name
            }).ToListAsync();
        }
        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            return await _roleManager.CreateAsync(new AppRole()
            {
                Name = roleName,
            });
        }
        public async Task<AppRole> RoleFindByIdAsync(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }
        public async Task UpdateRoleAsync(string id, string roleName)
        {
            var role = await RoleFindByIdAsync(id);
            role.Name = roleName;
            await _roleManager.UpdateAsync(role);
        }
        public async Task DeleteRoleAsync(string id)
        {
            var role = await RoleFindByIdAsync(id);
            await _roleManager.DeleteAsync(role);
        }
        public async Task<List<AssignToRoleViewModel>> GetAssignToRoleViewModel(string id)
        {
            var currentUser = await _userManager.FindByIdAsync(id);
            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(currentUser);
            var roleViewModelList = new List<AssignToRoleViewModel>();
            foreach (var role in roles)
            {
                var roleViewModel = new AssignToRoleViewModel()
                {
                    Id = role.Id,
                    Name = role.Name,
                };
                if (userRoles.Contains(role.Name))
                {
                    roleViewModel.Exist = true;
                }
                roleViewModelList.Add(roleViewModel);
            }
            return roleViewModelList;
        }
        public async Task<IdentityResult> AddToRoleAsync(string roleName, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return await _userManager.AddToRoleAsync(user, roleName);
        }
        public async Task<IdentityResult> RemoveFromRoleAsync(string roleName, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return await _userManager.RemoveFromRoleAsync(user, roleName);
        }
    }
}
