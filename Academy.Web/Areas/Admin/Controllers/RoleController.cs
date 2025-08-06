using Academy.Web.Areas.Admin.Controllers.Common;
using Academy.Web.Attributes;
using Academy.Application.Services.Interfaces;
using Academy.Application.Statics;
using Academy.Data.Statics;
using Academy.Domain.Enums.Role;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.Role;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Areas.Admin.Controllers;

public class RoleController(IRoleService roleService , IPermissionService permissionService) : AdminBaseController
{
    #region List

    [UserActivityLog(Description = ActivityMessages.RoleList),
     InvokePermission(PermissionName.RoleList)]
    [AdminSearchMarker(Title = AdminSearchTitles.RoleList)]
    public async Task<IActionResult> List(AdminSideFilterRoleViewModel filter) =>
        View(await roleService.AdminSideFilterAsync(filter));

    #endregion
    
    
    #region Create

    [UserActivityLog(Description = ActivityMessages.CreateRoleGet),
     InvokePermission(PermissionName.CreateRole)]
    public IActionResult Create() => PartialView("_CreatePartial");


    [HttpPost, ValidateAntiForgeryToken]
    [UserActivityLog(Description = ActivityMessages.CreateRolePost),
     InvokePermission(PermissionName.CreateRole)]
    public async Task<IActionResult> Create(AdminSideCreateRoleViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ErrorMessages.NullValue);
        var result = await roleService.CreateRoleAsync(model);
        if (result.IsFailure) return BadRequest(result.Message);
        TempData[ToasterSuccessMessage] = result.Message;
        return Ok();
    }

    #endregion

    #region Update

    [UserActivityLog(Description = ActivityMessages.UpdateRoleGet), 
     InvokePermission(PermissionName.UpdateRole)]
    public async Task<IActionResult> Update(short id)
    {
        var result = await roleService.FillModelForEditAsync(id);
        if (result.IsSuccess) return PartialView("_UpdatePartial", result.Value);
        TempData[ToasterErrorMessage] = result.Message;
        return RedirectToAction(nameof(List));
    }

    [HttpPost, ValidateAntiForgeryToken]
    [UserActivityLog(Description = ActivityMessages.UpdateRolePost),
     InvokePermission(PermissionName.UpdateRole)]
    public async Task<IActionResult> Update(AdminSideUpdateRoleViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ErrorMessages.NullValue);
        var result = await roleService.UpdateRoleAsync(model);
        if (result.IsFailure) return BadRequest(result.Message);
        TempData[ToasterSuccessMessage] = result.Message;
        return Ok();
    }

    #endregion

    #region DeleteOrRecover

    [UserActivityLog(Description = ActivityMessages.DeleteOrRecoverRole),
     InvokePermission(PermissionName.DeleteOrRecoverRole)]
    public async Task<IActionResult> DeleteOrRecover(short id)
    {
        var result = await roleService.DeleteOrRecoverRoleAsync(id);
        return result.IsFailure ? BadRequest(result) : Ok(result);
    }

    #endregion
    
    #region SetPermission

    [UserActivityLog(Description = ActivityMessages.SetPermissionToRoleGet), InvokePermission(PermissionName.SetPermissionToRole)]
    public async Task<IActionResult> SetPermissionToRole(short id, RoleSection section)
    {
        var permissions = await permissionService.GetPermissionsForRoleAsync(section);
        var selectedPermissions = await roleService.GetRoleSelectedPermissionsAsync(id);
        return View(new AdminSideSetPermissionToRoleViewModel { RoleId = id, Permissions = permissions, SelectedPermissionIds = selectedPermissions });
    }

    [HttpPost, ValidateAntiForgeryToken]
    [UserActivityLog(Description = ActivityMessages.SetPermissionToRolePost), InvokePermission(PermissionName.SetPermissionToRole)]
    public async Task<IActionResult> SetPermissionToRole(AdminSideSetPermissionToRoleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData[ToasterErrorMessage] = ErrorMessages.NullValue;
            return RedirectToRefererUrl();
        }

        var result = await roleService.SetPermissionToRole(model);

        if (result.IsFailure)
        {
            TempData[ToasterErrorMessage] = result.Message;
            return RedirectToRefererUrl();
        }

        TempData[ToasterSuccessMessage] = result.Message;
        return RedirectToAction(nameof(List));
    }

    #endregion


}