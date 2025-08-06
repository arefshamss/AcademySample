using Academy.Web.Areas.Admin.Controllers.Common;
using Academy.Web.Attributes;
using Academy.Application.Services.Interfaces;
using Academy.Application.Statics;
using Academy.Data.Statics;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.SmsProvider;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Areas.Admin.Controllers
{
    [InvokePermission(PermissionName.SmsProviderManagement)]
    public class SmsProviderController : AdminBaseController
    {
        #region Fields

        private readonly ISmsProviderService _providerService;

        #endregion

        #region Ctor

        public SmsProviderController(ISmsProviderService providerService)
        {
            _providerService = providerService;
        }

        #endregion

        #region List

        [UserActivityLog(Description = ActivityMessages.SmsProvidersList), InvokePermission(PermissionName.SmsProviderList)]
        [AdminSearchMarker(Title = AdminSearchTitles.SmsProvidersList)]
        public async Task<IActionResult> List()
            => View(await _providerService.GetSmsProvidersAsync());

        #endregion

        #region Update

        [UserActivityLog(Description = ActivityMessages.UpdateSmsProviderGet), InvokePermission(PermissionName.UpdateSmsProvider)]
        public async Task<IActionResult> Update(short id)
        {
            var result = await _providerService.FillModelForEditAsync(id);

            if (result.IsFailure)
            {
                TempData[ToasterErrorMessage] = result.Message;
                return RedirectToAction(nameof(List));
            }

            return View(result.Value);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [UserActivityLog(Description = ActivityMessages.UpdateSmsProviderPost), InvokePermission(PermissionName.UpdateSmsProvider)]
        public async Task<IActionResult> Update(AdminSideUpdateSmsProviderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData[ToasterErrorMessage] = ErrorMessages.NullValue;
                return View(model);
            }

            var result = await _providerService.UpdateProviderAsync(model);

            if (result.IsFailure)
            {
                TempData[ToasterErrorMessage] =result.Message;
                return View(model);
            }

            TempData[ToasterSuccessMessage] = result.Message;
            return RedirectToAction(nameof(List));
        }

        #endregion

    }
}
