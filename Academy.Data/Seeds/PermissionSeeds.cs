using Academy.Data.Statics;
using Academy.Domain.Enums.Role;
using Academy.Domain.Models.Permissions;

namespace Academy.Data.Seeds;

public static class PermissionSeeds
{
    public static List<Permission> ApplicationPermissions { get; } =
        new()
        {
            #region Admin

            #region Admin Dashboard

            new()
            {
                Id = 1,
                ParentId = null,
                UniqueName = PermissionName.AdminDashboard,
                DisplayName = "داشبورد ادمین",
                RoleSection = RoleSection.Admin, CreatedDate = SeedStaticDateTime.Date
            },
            
            new()
            {
                Id = 2,
                ParentId = 1,
                UniqueName = PermissionName.ClearCache,
                DisplayName = "پاک کردن cache سایت",
                RoleSection = RoleSection.Admin, CreatedDate = SeedStaticDateTime.Date
            },

            #endregion
            
            #endregion

            // available id : 2
            
        };
}