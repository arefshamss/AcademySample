using Academy.Web.Areas.Admin.Controllers.Common;
using Academy.Web.Attributes;
using Academy.Application.Services.Interfaces;
using Academy.Application.Statics;
using Academy.Data.Statics;
using Academy.Domain.ViewModels.UserActivity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Academy.Web.Areas.Admin.Controllers;

[InvokePermission(PermissionName.UserActivityManagement)]
public class UserActivityController(IUserActivityService userActivityService) : AdminBaseController
{
    [UserActivityLog(Description = ActivityMessages.UserActivityLogsList),
     InvokePermission(PermissionName.UserActivityList)]
    [AdminSearchMarker(Title = AdminSearchTitles.UserActivityLogsList)]
    public async Task<IActionResult> List(FilterUserActivityViewModel filter)
    {
        var result = await userActivityService.FilterAsync(filter);
        if (result.IsFailure)
            ShowToasterErrorMessage(result.Message);

        return View(result.Value);
    }

    [UserActivityLog(Description = ActivityMessages.UserActivityLogDetail),
     InvokePermission(PermissionName.UserActivityDetail)]
    public async Task<IActionResult> Detail(ObjectId id)
    {
        var result = await userActivityService.GetDetailAsync(id);

        return result.IsSuccess ? PartialView("_DetailPartial", result.Value) : BadRequest(result.Message);
    }
}