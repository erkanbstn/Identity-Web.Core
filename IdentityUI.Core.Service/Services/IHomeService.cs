using IdentityUI.Core.Core.ViewModels;
using IdentityUI.Core.Repository.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityUI.Core.Service.Services
{
    public interface IHomeService
    {
        Task<List<RoleViewModel>> RoleViewModelToListAsync();
        Task<List<UserAdminViewModel>> UserToListAsync();
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<AppRole> RoleFindByIdAsync(string id);
        Task<List<RoleViewModel>> ByRoleViewModelToListAsync(string id);
        Task UpdateRoleAsync(string id, string roleName);
        Task DeleteRoleAsync(string id);
        Task<List<AssignToRoleViewModel>> GetAssignToRoleViewModel(string id);
        Task<IdentityResult> AddToRoleAsync(string roleName, string id);
        Task<IdentityResult> RemoveFromRoleAsync(string roleName, string id);
    }
}
