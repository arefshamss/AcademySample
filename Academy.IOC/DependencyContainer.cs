using Academy.Application.Senders.Mail;
using Academy.Application.Services.Implementation;
using Academy.Application.Services.Interfaces;
using Academy.Data.Repositories;
using Academy.Domain.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Academy.IOC;

public static class DependencyContainer
{
       public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            #region Services
            
            services.AddScoped<IMemoryCacheService, MemoryCacheService>();
            services.AddScoped<IEmailSmtpService, EmailSmtpService>();
            services.AddScoped<IMailSender, MailSender>();
            services.AddScoped<ICaptchaService, CaptchaService>();
            services.AddScoped<ICaptchaSettingService, CaptchaSettingService>();
            services.AddScoped<ISmsProviderService, SmsProviderService>();
            services.AddScoped<ISmsSettingService, SmsSettingService>();
            services.AddScoped<ISmsSenderService, SmsSenderService>();
            services.AddScoped<IAdminSearchEntryService, AdminSearchEntryService>();
            services.AddScoped<IApplicationLogService, ApplicationLogService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserActivityService, UserActivityService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();
            services.AddScoped<ISiteSettingService, SiteSettingService>();
            services.AddScoped<ICourseCategoryService , CourseCategoryService>();   
            services.AddScoped<ITicketService , TicketService>();     
            services.AddScoped<ITicketMessageService , TicketMessageService>();     

            #endregion

            #region Repositories
            
            services.AddScoped<IAdminSearchEntryRepository , AdminSearchEntryRepository>();
            services.AddScoped<IApplicationLogRepository , ApplicationLogRepository>();
            services.AddScoped<ICaptchaRepository, CaptchaRepository>();
            services.AddScoped<ICaptchaSettingRepository, CaptchaSettingRepository>();
            services.AddScoped<IEmailSmtpRepository, EmailSmtpRepository>();
            services.AddScoped<IPermissionRepository , PermissionRepository>();
            services.AddScoped<IRoleRepository , RoleRepository>();
            services.AddScoped<ISiteSettingRepository, SiteSettingRepository>();
            services.AddScoped<ISmsProviderRepository, SmsProviderRepository>();
            services.AddScoped<ISmsSettingRepository, SmsSettingRepository>();
            services.AddScoped<IUserActivityRepository , UserActivityRepository>();
            services.AddScoped<IUserRoleMappingRepository , UserRoleMappingRepository>();      
            services.AddScoped<IUserRepository , UserRepository>();      
            services.AddScoped<IUserInformationRepository , UserInformationRepository>();      
            services.AddScoped<ICourseCategoryRepository , CourseCategoryRepository>();     
            services.AddScoped<ITicketRepository , TicketRepository>();     
            services.AddScoped<ITicketMessageRepository , TicketMessageRepository>();     
            
            #endregion

            return services;
        }
}