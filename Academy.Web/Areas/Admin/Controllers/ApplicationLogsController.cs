using Academy.Web.Areas.Admin.Controllers.Common;
using Academy.Web.Attributes;
using Academy.Application.Services.Interfaces;
using Academy.Application.Statics;
using Academy.Data.Statics;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.ApplicationLogs;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Academy.Web.Areas.Admin.Controllers;

[InvokePermission(PermissionName.ApplicationLogManagement)]
public class ApplicationLogsController(IApplicationLogService applicationLogService) : AdminBaseController
{
    
    #region List

    [InvokePermission(PermissionName.ApplicationLogsList)]
    [UserActivityLog(Description = ActivityMessages.GetApplicationLogsList)]
    [AdminSearchMarker(Title = AdminSearchTitles.ApplicationLogsList)]
    public async Task<IActionResult> List(FilterApplicationLogsViewModel filter)
    {
        var result = await applicationLogService.FilterAsync(filter);
        if (result.IsFailure)
            TempData[ToasterErrorMessage] = result.Message;
            
        return View(result.Value);
    }

    #endregion
    
    #region Delete

    [InvokePermission(PermissionName.DeleteApplicationLog)]
    [UserActivityLog(Description = ActivityMessages.DeleteApplicationLog)]
    public async Task<IActionResult> Delete(ObjectId id)
    {
        var result = await applicationLogService.DeleteAsync(id);
        return result.IsFailure ? BadRequest(result) : Ok(result);
    }
    
    [InvokePermission(PermissionName.DeleteApplicationLog)]
    [UserActivityLog(Description = ActivityMessages.DeleteApplicationLogs)]
    public async Task<IActionResult> DeleteAll()
    {
        await applicationLogService.DeleteAllAsync();
        TempData[ToasterSuccessMessage] = SuccessMessages.DeleteSuccess;
        return RedirectToAction("List");
    }  
    #endregion

    #region Detail

    [InvokePermission(PermissionName.ApplicationLogDetail)]
    [UserActivityLog(Description = ActivityMessages.GetApplicationLogDetail)]
    public async Task<IActionResult> Detail(ObjectId id)
    {
        var result = await applicationLogService.GetDetailAsync(id);
        return result.IsFailure ? BadRequest(result.Message) : PartialView("_DetailPartial" , result.Value);
    }

    #endregion
}