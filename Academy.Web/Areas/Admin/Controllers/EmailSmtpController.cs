using Academy.Web.Areas.Admin.Controllers.Common;
using Academy.Web.Attributes;
using Academy.Application.Services.Interfaces;
using Academy.Application.Statics;
using Academy.Data.Statics;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.EmailSmtp;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Areas.Admin.Controllers;


[InvokePermission(PermissionName.EmailSmtpManagement)]
public class EmailSmtpController(IEmailSmtpService emailSmtpService) : AdminBaseController
{
        #region List

        [UserActivityLog(Description = ActivityMessages.EmailSmtpList),InvokePermission(PermissionName.EmailSmtpList)]
        [AdminSearchMarker(Title = AdminSearchTitles.EmailSmtpList)]
        public async Task<IActionResult> List(FilterAdminSideEmailSmtpViewModel filter)
        {
            var result = await emailSmtpService.FilterAsync(filter);
            return View(result);
        }

        #endregion

        #region Create

        [UserActivityLog(Description = ActivityMessages.CreateEmailSmtpGet), InvokePermission(PermissionName.CreateEmailSmtp)]
        [AdminSearchMarker(Title = AdminSearchTitles.CreateEmailSmtp)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, UserActivityLog(Description = ActivityMessages.CreateEmailSmtpPost), InvokePermission(PermissionName.CreateEmailSmtp)]
        public async Task<IActionResult> Create(AdminSideCreateEmailSmtpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData[ToasterErrorMessage] = ErrorMessages.NullValue;
                return View(model);
            }

            var result = await emailSmtpService.CreateAsync(model);

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

        [UserActivityLog(Description = ActivityMessages.UpdateEmailSmtpGet), InvokePermission(PermissionName.UpdateEmailSmtp)]
        public async Task<IActionResult> Update(short id)
        {
            var result = await emailSmtpService.FillModelForEditAsync(id);
            if (result.IsFailure)
            {
                TempData[ToasterErrorMessage] = result.Message;
                return RedirectToRefererUrl();
            }

            return View(result.Value);
        }

        [HttpPost, ValidateAntiForgeryToken, UserActivityLog(Description = ActivityMessages.UpdateEmailSmtpPost), InvokePermission(PermissionName.UpdateEmailSmtp)]
        public async Task<IActionResult> Update(AdminSideUpdateEmailSmtpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData[ToasterErrorMessage] = ErrorMessages.NullValue;
                return View(model);
            }

            var result = await emailSmtpService.UpdateAsync(model);

            if (result.IsFailure)
            {
                TempData[ToasterErrorMessage] = result.Message;
                return View(model);
            }

            TempData[ToasterSuccessMessage] = result.Message;
            return RedirectToAction(nameof(List));
        }

        #endregion

        #region DeleteOrRecover

        [UserActivityLog(Description = ActivityMessages.DeleteOrRecoverEmailSmtp), InvokePermission(PermissionName.DeleteOrRecoverEmailSmtp)]
        public async Task<IActionResult> DeleteOrRecover(short id)
        {
            var result = await emailSmtpService.SoftDeleteOrRecoverAsync(id);

            return result.IsFailure ? BadRequest(result) : Ok(result);
        }

        #endregion
}