using Academy.Web.Areas.Admin.Controllers.Common;
using Academy.Web.Attributes;
using Academy.Application.Services.Interfaces;
using Academy.Application.Statics;
using Academy.Data.Statics;
using Academy.Domain.Enums.EmailSmtp;
using Academy.Domain.ViewModels.SiteSetting;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Areas.Admin.Controllers;

public class SiteSettingsController(ISiteSettingService siteSettingService,
    IEmailSmtpService emailSmtpService) : AdminBaseController
{
    [HttpGet]
    [UserActivityLog(Description = ActivityMessages.UpdateSiteSettingGet),InvokePermission(PermissionName.UpdateSiteSettings)]
    [AdminSearchMarker(Title = AdminSearchTitles.UpdateSiteSetting)]
    public async Task<IActionResult> ChangeSiteSetting()
    {
        var siteSetting = await siteSettingService.GetSiteSettingForUpdateAsync();

        ViewData["MailServersSmtp"] =
            (await emailSmtpService.GetForSelectList(EmailSmtpType.MailServer, siteSetting.DefaultMailServerSmtpId))
            .Value;

        ViewData["SimplesSmtp"] =
            (await emailSmtpService.GetForSelectList(EmailSmtpType.Simple, siteSetting.DefaultSimpleSmtpId))
            .Value;

        return View(siteSetting);
    }


    [HttpPost, ValidateAntiForgeryToken]
    [UserActivityLog(Description = ActivityMessages.UpdateSiteSettingPost), InvokePermission(PermissionName.UpdateSiteSettings)]
    public async Task<IActionResult> ChangeSiteSetting(UpdateSiteSettingViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["MailServersSmtp"] =
                (await emailSmtpService.GetForSelectList(EmailSmtpType.MailServer, model.DefaultMailServerSmtpId))
                .Value;

            ViewData["SimplesSmtp"] =
                (await emailSmtpService.GetForSelectList(EmailSmtpType.Simple, model.DefaultSimpleSmtpId))
                .Value;

            return View(model);
        }

        var updateResult = await siteSettingService.UpdateSiteSettingAsync(model);

        if (updateResult.IsSuccess)
            TempData[ToasterSuccessMessage] = updateResult.Message;
        else
            TempData[ToasterErrorMessage] = updateResult.Message;

        return RedirectToAction(nameof(ChangeSiteSetting));
    }
}