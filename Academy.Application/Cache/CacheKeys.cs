namespace Academy.Application.Cache;

public static class CacheKeys
{
    #region SiteSetting
    
    public static readonly CacheKey SiteSettingPrefix = new ("SiteSetting"); 
    public static readonly CacheKey SiteSetting = new("SiteSettingModel");
    public static readonly CacheKey UpdateSiteSettingCache = new("SiteSettingUpdate");
    
    #endregion
    
    #region SmtpEmail
    
    public static readonly CacheKey SmtpEmailPrefix = new ("SmtpEmail");
        
    public static readonly CacheKey DefaultSimpleSmtpId = new("SmtpEmailSimpleSmtpId");
    
    public static readonly CacheKey DefaultMailServerSmtpId = new("SmtpEmailMailServerSmtpId");
    
    #endregion
    
    #region Captcha

    public static readonly CacheKey CaptchaPrefix = new("Captcha");

    public static readonly CacheKey Captcha = new("Captcha-{0}");

    public static readonly CacheKey ArCaptchaSiteKey = new("CaptchaArSiteKey");


    #endregion

    #region Permissions

    public static readonly CacheKey UserRoleMappings = new("UserRoleMappings-{0}");
    
    public static readonly CacheKey RolePermissionMappings = new("RolePermissionMappings");

    #endregion

    #region Banners

    public static readonly CacheKey SiteBannersBySection = new ("Banners-{0}");
    
    #endregion
}