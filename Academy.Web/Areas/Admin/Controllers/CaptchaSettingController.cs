using Academy.Web.Areas.Admin.Controllers.Common;
using Academy.Web.Attributes;
using Academy.Application.Services.Interfaces;
using Academy.Application.Statics;
using Academy.Data.Statics;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.CaptchaSetting;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Areas.Admin.Controllers;

[InvokePermission(PermissionName.CaptchaSettingManagement)]

public class CaptchaSettingController(ICaptchaSettingService captchaSettingService) : AdminBaseController
{
    #region Update

    [UserActivityLog(Description = ActivityMessages.UpdateCaptchaSettingGet), InvokePermission(PermissionName.UpdateCaptchaSetting)]
    [AdminSearchMarker(Title = AdminSearchTitles.UpdateCaptchaSetting)]
    public async Task<IActionResult> Update()
    {
        var result = await captchaSettingService.FillModelForEditAsync();
        return View(result);
    }

    [HttpPost, ValidateAntiForgeryToken]
    [UserActivityLog(Description = ActivityMessages.UpdateCaptchaSettingPost), InvokePermission(PermissionName.UpdateCaptchaSetting)]
    public async Task<IActionResult> Update(AdminSideUpdateCaptchaSettingViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData[ToasterErrorMessage] = ErrorMessages.NullValue;
            return View(model);
        }

        var result = await captchaSettingService.UpdateCaptchaSettingsAsync(model);

        if (result.IsFailure)
        {
            TempData[ToasterErrorMessage] = result.Message;
            return View(model);
        }

        TempData[ToasterSuccessMessage] = result.Message;
        return RedirectToAction(nameof(Update));
    }

    #endregion
}