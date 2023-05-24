using IdentityUI.Core.Models;
using IdentityUI.Core.Permissions;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
namespace IdentityUI.Core.Seeds
{
    public  class PermissionSeed
    {
        public static async Task Seed(RoleManager<AppRole> roleManager)
        {
            var hasBasicRole = await roleManager.RoleExistsAsync("BasicRole");
            var hasAdvancedRole = await roleManager.RoleExistsAsync("AdvancedRole");
            var hasAdminRole = await roleManager.RoleExistsAsync("AdminRole");
            if (!hasBasicRole)
            {
                await roleManager.CreateAsync(new AppRole() { Name = "BasicRole" });
                var basicRole = await roleManager.FindByNameAsync("BasicRole");
                await AddReadPermission(basicRole, roleManager);
            }
            if (!hasAdvancedRole)
            {
                await roleManager.CreateAsync(new AppRole() { Name = "AdvancedRole" });
                var basicRole = await roleManager.FindByNameAsync("AdvancedRole");
                await AddReadPermission(basicRole, roleManager);
                await AddCreateAndUpdatePermission(basicRole, roleManager);
            }
            if (!hasAdminRole)
            {
                await roleManager.CreateAsync(new AppRole() { Name = "AdminRole" });
                var basicRole = await roleManager.FindByNameAsync("AdminRole");
                await AddReadPermission(basicRole, roleManager);
                await AddCreateAndUpdatePermission(basicRole, roleManager);
                await AddDeletePermission(basicRole, roleManager);
            }
        }
        public static async Task AddReadPermission(AppRole appRole,RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(appRole, new Claim("Permission", Permission.Stock.Read));
            await roleManager.AddClaimAsync(appRole, new Claim("Permission", Permission.Order.Read));
            await roleManager.AddClaimAsync(appRole, new Claim("Permission", Permission.Catalog.Read));
        }
        public static async Task AddDeletePermission(AppRole appRole, RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(appRole, new Claim("Permission", Permission.Stock.Delete));
            await roleManager.AddClaimAsync(appRole, new Claim("Permission", Permission.Order.Delete));
            await roleManager.AddClaimAsync(appRole, new Claim("Permission", Permission.Catalog.Delete));
        }
        public static async Task AddCreateAndUpdatePermission(AppRole appRole, RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(appRole, new Claim("Permission", Permission.Stock.Create));
            await roleManager.AddClaimAsync(appRole, new Claim("Permission", Permission.Order.Create));
            await roleManager.AddClaimAsync(appRole, new Claim("Permission", Permission.Catalog.Create));

            await roleManager.AddClaimAsync(appRole, new Claim("Permission", Permission.Stock.Update));
            await roleManager.AddClaimAsync(appRole, new Claim("Permission", Permission.Order.Update));
            await roleManager.AddClaimAsync(appRole, new Claim("Permission", Permission.Catalog.Update));
        }
    }
}
