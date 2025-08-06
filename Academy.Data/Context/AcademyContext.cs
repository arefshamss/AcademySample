using Academy.Data.Seeds;
using Academy.Domain.Models.Captcha;
using Academy.Domain.Models.Category;
using Academy.Domain.Models.EmailSmtp;
using Academy.Domain.Models.Permissions;
using Academy.Domain.Models.Roles;
using Academy.Domain.Models.SiteSettings;
using Academy.Domain.Models.SmsProvider;
using Academy.Domain.Models.SmsSetting;
using Academy.Domain.Models.Ticket;
using Academy.Domain.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Academy.Data.Context
{
    public class AcademyContext(DbContextOptions<AcademyContext> options)
        : DbContext(options)
    {
        
        #region SiteSetting

        public DbSet<SiteSettings> SiteSettings { get; set; }

        #endregion

        #region EmailSMTP

        public DbSet<EmailSmtp> EmailSmtps { get; set; }

        #endregion
        
        #region Captcha

        public DbSet<Captcha> Captchas { get; set; }

        public DbSet<CaptchaSetting> CaptchaSettings { get; set; }

        #endregion
        
        #region Permissions

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<RolePermissionMapping> RolePermissionMappings { get; set; }

        #endregion

        #region Roles

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRoleMapping> UserRoleMappings { get; set; }

        #endregion

        #region Users
        
        public DbSet<User> Users { get; set; }
        
        public DbSet<UserInformation> UserInformations { get; set; }
        
        #endregion
        
        #region SmsProvider

        public DbSet<SmsProvider> SmsProviders { get; set; }

        #endregion

        #region SmsSetting

        public DbSet<SmsSetting> SmsSettings { get; set; }

        #endregion
        
        #region Category

        public DbSet<CourseCategory> CourseCategories { get; set; }

        #endregion
        
        #region Ticket
        
        public DbSet<Ticket> Tickets { get; set; }
        
        public DbSet<TicketMessage> TicketMessages { get; set; }
        
        #endregion
        
        #region OnModelCreating

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddApplicationSeeds()
                .ApplyConfigurationsFromAssembly(GetType().Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}