using Academy.Web.Areas.Admin.Controllers.Common;
using Academy.Web.Attributes;
using Academy.Application.Services.Interfaces;
using Academy.Application.Statics;
using Academy.Data.Statics;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.Captcha;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Areas.Admin.Controllers;

public class CaptchaController(ICaptchaService captchaService) : AdminBaseController
{
        #region List

        [UserActivityLog(Description = ActivityMessages.CaptchasList), InvokePermission(PermissionName.CaptchaList)]
        [AdminSearchMarker(Title = AdminSearchTitles.CaptchaList)]
        public async Task<IActionResult> List()
        {
            var result = await captchaService.GetAllCaptchaAsync();
            return View(result);
        }

        #endregion

        #region Create

        [UserActivityLog(Description = ActivityMessages.CreateCaptchaGet),
         InvokePermission(PermissionName.CreateCaptcha) , HttpGet]
        [AdminSearchMarker(Title = AdminSearchTitles.CreateCaptcha)]
        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        [UserActivityLog(Description = ActivityMessages.CreateCaptchaPost), InvokePermission(PermissionName.CreateCaptcha)]
        public async Task<IActionResult> Create(AdminSideCreateCaptchaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData[ToasterErrorMessage] = ErrorMessages.NullValue;
                return View(model);
            }

            var result=await captchaService.CreateCaptchaAsync(model);

            if (result.IsFailure)
            {
                TempData[ToasterErrorMessage] = result.Message;
                return View(model);
            }

            TempData[ToasterSuccessMessage] = result.Message;
            return RedirectToAction(nameof(List));
        }

        #endregion

        #region Update

        [UserActivityLog(Description = ActivityMessages.UpdateCaptchaGet), InvokePermission(PermissionName.UpdateCaptcha)]
        public async Task<IActionResult> Update(short id)
        {
            var result = await captchaService.FillModelForEditAsync(id);
            if (result.IsFailure)
            {
                TempData[ToasterErrorMessage] = result.Message;
                return RedirectToRefererUrl();
            }

            return View(result.Value);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [UserActivityLog(Description = ActivityMessages.UpdateCaptchaPost), InvokePermission(PermissionName.UpdateCaptcha)]
        public async Task<IActionResult> Update(AdminSideUpdateCaptchaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData[ToasterErrorMessage] = ErrorMessages.NullValue;
                return View(model);
            }

            var result = await captchaService.UpdateCaptchaAsync(model);

            if (result.IsFailure)
            {
                TempData[ToasterErrorMessage] = result.Message;
                return View(model);
            }

            TempData[ToasterSuccessMessage] = result.Message;
            return RedirectToAction(nameof(List));
        }

        #endregion
}