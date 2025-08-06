using Academy.Domain.Models.Captcha;
using Academy.Domain.Models.Permissions;
using Academy.Domain.Models.Roles;
using Academy.Domain.Models.SiteSettings;
using Academy.Domain.Models.SmsProvider;
using Academy.Domain.Models.SmsSetting;
using Microsoft.EntityFrameworkCore;

namespace Academy.Data.Seeds;

public static class ApplicationSeeds
{
    public static ModelBuilder AddApplicationSeeds(this ModelBuilder modelBuilder)
    {
        #region Permission

        modelBuilder.Entity<Permission>().HasData(PermissionSeeds.ApplicationPermissions);

        #endregion

        #region SmsProider

        modelBuilder.Entity<SmsProvider>().HasData(SmsProviderSeeds.SmsProviders);

        #endregion

        #region SmsSetting

        modelBuilder.Entity<SmsSetting>().HasData(SmsSettingSeeds.SmsSettingList);

        #endregion
        

        #region SiteSetting

        modelBuilder.Entity<SiteSettings>().HasData(SiteSettingSeeds.SiteSettingsList);

        #endregion

        #region Role

        modelBuilder.Entity<Role>().HasData(RoleSeeds.RoleSeed);

        #endregion

        #region Role Permissions 

        foreach (var permission in PermissionSeeds.ApplicationPermissions)
        {
            modelBuilder.Entity<RolePermissionMapping>().HasData(new List<RolePermissionMapping>
            {
                new(roleId:1,permission.Id)
            });
        }

        #endregion

        #region Captcha

        modelBuilder.Entity<Captcha>().HasData(CaptchaSeeds.CaptchaList);

        modelBuilder.Entity<CaptchaSetting>().HasData(CaptchaSettingSeeds.CaptchaSettingList);

        #endregion

        return modelBuilder;
    }
}