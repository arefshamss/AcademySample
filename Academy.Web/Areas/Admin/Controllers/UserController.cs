using Academy.Web.Areas.Admin.Controllers.Common;
using Academy.Web.Attributes;
using Academy.Application.Extensions;
using Academy.Application.Mapper.UserMappings;
using Academy.Application.Services.Interfaces;
using Academy.Application.Statics;
using Academy.Data.Statics;
using Academy.Domain.ViewModels.User.Admin;
using Academy.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Areas.Admin.Controllers;

[InvokePermission(PermissionName.UsersManagement)]
public class UserController(IUserService userService) : AdminBaseController
{
    [HttpGet]
    [UserActivityLog(Description = ActivityMessages.UsersList), InvokePermission(PermissionName.UsersList)]
    [AdminSearchMarker(Title = AdminSearchTitles.UserList)]
    public async Task<IActionResult> List(AdminFilterUserViewModel filter)
    {
        var result = await userService.FilterAsync(filter);
        return View(result.Value);
    }
    
    [HttpGet]
    [UserActivityLog(Description = ActivityMessages.UsersList), InvokePermission(PermissionName.UsersList)]
    public async Task<IActionResult> ListPartial(AdminFilterUserViewModel filter)
    {
        filter.TakeEntity = 5;
        var result = await userService.FilterAsync(filter);
        result.Value.FormId = "filter-users";
        return PartialView("_ListPartial", result.Value);
    }
    
    #region ExportData

    [UserActivityLog(Description = ActivityMessages.UserExportExcel), InvokePermission(PermissionName.UserExportExcel),HttpGet]
    public async Task<IActionResult> ExportExcel(AdminFilterUserViewModel filter)
    {
        var result = await userService.ExportDataAsync(filter);
        if (!result.IsSuccess) return BadRequest(result.Message);

        return Excel(result.Value.GenerateExcel<AdminUserExportDataViewModel>());
    }

    [UserActivityLog(Description = ActivityMessages.UserExportPdf), InvokePermission(PermissionName.UserExportPdf),HttpGet]
    public async Task<IActionResult> ExportPdf(AdminFilterUserViewModel filter)
    {
        var result = await userService.ExportDataAsync(filter);
        if (!result.IsSuccess) return BadRequest(result.Message);

        return Pdf(result.Value.GeneratePdf<AdminUserExportDataViewModel>());
    }
    
    [UserActivityLog(Description = ActivityMessages.UserMobileExportExcel), InvokePermission(PermissionName.UserMobileExportExcel),HttpGet]
    public async Task<IActionResult> ExportMobileExcel(AdminFilterUserViewModel filter)
    {
        var result = await userService.ExportMobileDataAsync(filter);
        if (!result.IsSuccess) return BadRequest(result.Message);

        return Excel(result.Value.GenerateExcel<AdminUserExportMobileDataViewModel>());
    }

    #endregion
    
    #region Details

    [UserActivityLog(Description = ActivityMessages.UserDetailsGet), InvokePermission(PermissionName.UserDetails),HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var result = await userService.GetByIdAsync(id);

        if (result.IsFailure)
        {
            ShowToasterErrorMessage(result.Message);
            return RedirectToAction(nameof(List));
        }

        return View(result.Value);
    }

    #endregion
    
    #region Create

    [UserActivityLog(Description = ActivityMessages.CreateUserGet), InvokePermission(PermissionName.CreateUser),HttpGet]
    [AdminSearchMarker(Title = AdminSearchTitles.CreateUser)]
    public IActionResult Create() =>
        View();


    [HttpPost, ValidateAntiForgeryToken]
    [UserActivityLog(Description = ActivityMessages.CreateUserPost), InvokePermission(PermissionName.CreateUser)]
    public async Task<IActionResult> Create(AdminCreateUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ShowToasterErrorMessage(ModelState.GetModelErrorsAsString());
            return View(model);
        }

        var result = await userService.CreateAsync(model);

        if (result.IsFailure)
        {
            ShowToasterErrorMessage(result.Message);
            return View(model);
        }

        ShowToasterSuccessMessage(result.Message);

        return RedirectToAction(nameof(List));
    }

    #endregion
    
    #region Update
    
    [UserActivityLog(Description = ActivityMessages.UpdateUserGet), InvokePermission(PermissionName.UpdateUser),HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var result = await userService.FillModelForUpdateAsync(id);
    
        if (result.IsFailure)
        {
            ShowToasterErrorMessage(result.Message);
            return RedirectToAction(nameof(List));
        }
    
        return View(result.Value);
    }
    
    [HttpPost, ValidateAntiForgeryToken]
    [UserActivityLog(Description = ActivityMessages.UpdateUserPost), InvokePermission(PermissionName.UpdateUser)]
    public async Task<IActionResult> Update(AdminUpdateUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ShowToasterErrorMessage(ModelState.GetModelErrorsAsString());
            return View(model);
        }
    
        var result = await userService.UpdateAsync(model);
    
        if (result.IsFailure)
        {
            ShowToasterErrorMessage(result.Message);
            return View(model);
        }
    
        ShowToasterSuccessMessage(result.Message);
    
        return RedirectToAction(nameof(List));
    }
    
    #endregion
    
    #region DeleteOrRecover
    
    [UserActivityLog(Description = ActivityMessages.DeleteOrRecoverUser), InvokePermission(PermissionName.DeleteOrRecoverUser),HttpGet]
    public async Task<IActionResult> DeleteOrRecover(int id)
    {
        var result = await userService.DeleteOrRecoverAsync(id);
        return result.IsSuccess ? Ok(result.Message) : BadRequest(result.Message);
    }
    
    #endregion

}