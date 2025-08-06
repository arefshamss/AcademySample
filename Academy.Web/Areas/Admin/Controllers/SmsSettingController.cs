using Academy.Web.Areas.Admin.Controllers.Common;
using Academy.Web.Attributes;
using Academy.Application.Services.Interfaces;
using Academy.Application.Statics;
using Academy.Data.Statics;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.SmsSetting;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Areas.Admin.Controllers
{
    [InvokePermission(PermissionName.SmsSettingManagement)]
    public class SmsSettingController : AdminBaseController
    {
        #region Fields

        private readonly ISmsSettingService _smsSettingService;

        #endregion

        #region Ctor

        public SmsSettingController(ISmsSettingService smsSettingService)
        {
            _smsSettingService = smsSettingService;
        }

        #endregion

        #region Update

        [UserActivityLog(Description = ActivityMessages.UpdateSmsSettingGet),
         InvokePermission(PermissionName.UpdateSmsSetting)]
        [AdminSearchMarker(Title = AdminSearchTitles.UpdateSmsSetting)]
        public async Task<IActionResult> Update() => View(await _smsSettingService.FillModelForEditAsync());

        [HttpPost, ValidateAntiForgeryToken]
        [UserActivityLog(Description = ActivityMessages.UpdateSmsSettingPost), InvokePermission(PermissionName.UpdateSmsSetting)]
        public async Task<IActionResult> Update(AdminSideUpdateSmsSettingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData[ToasterErrorMessage] = ErrorMessages.NullValue;
                return View(model);
            }

            var result = await _smsSettingService.UpdateSmsSettingAsync(model);

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
}
