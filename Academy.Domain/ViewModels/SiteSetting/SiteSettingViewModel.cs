using System.ComponentModel.DataAnnotations;
using Academy.Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace Academy.Domain.ViewModels.SiteSetting;

public class SiteSettingViewModel
{
    [Display(Name = "نام سایت")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    public string SiteName { get; set; }
    
    
    [Display(Name = "کپی رایت")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    public string Copyright { get; set; }

    [Display(Name = "لوگوی سایت")]
    public string? Logo { get; set; }

    [Display(Name = "فاو آیکون")]
    public string? Favicon { get; set; }
    
    [Display(Name = "Simple Smtp")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    public short? DefaultSimpleSmtpId { get; set; }
    
    [Display(Name = "MailServer Smtp")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    public short? DefaultMailServerSmtpId { get; set; }
}

public class UpdateSiteSettingViewModel : SiteSettingViewModel
{
    public IFormFile? LogoFile { get; set; }
    
    public  IFormFile? FavIconFile { get; set; }
}