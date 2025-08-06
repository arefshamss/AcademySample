using Academy.Domain.Enums.Role;
using Academy.Domain.Models.Roles;

namespace Academy.Data.Seeds
{
    public class RoleSeeds
    {
        public static List<Role> RoleSeed { get; } = new(){
            new()
            {
                Id = 1,
                RoleName = "مدیر سیستم",
                RoleSection = RoleSection.Admin,
                IsDeleted = false
            },
            new()
            {
                Id = 2,
                RoleName = "مدرس",
                RoleSection = RoleSection.Master,
                IsDeleted = false
            }
        };
    }
}
