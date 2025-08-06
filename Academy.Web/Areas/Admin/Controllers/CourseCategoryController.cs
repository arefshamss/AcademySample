using Academy.Web.Areas.Admin.Controllers.Common;
using Academy.Web.Attributes;
using Academy.Application.Services.Interfaces;
using Academy.Application.Statics;
using Academy.Data.Statics;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.CourseCategory.Admin;
using Academy.Domain.ViewModels.Role;
using Academy.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.Web.Areas.Admin.Controllers;

public class CourseCategoryController(ICourseCategoryService courseCategoryService) : AdminBaseController
{
    [HttpGet]
    [UserActivityLog(Description = ActivityMessages.CourseCategoriesList), InvokePermission(PermissionName.CourseCategoriesList)]
    [AdminSearchMarker(Title = AdminSearchTitles.CourseCategoriesList)]
    public async Task<IActionResult> List(AdminFilterCourseCategoryViewModel filter)
    {
        var result = await courseCategoryService.FilterAsync(filter);
        
        return View(result.Value);
    }

    
    #region Create
    
    [UserActivityLog(Description = ActivityMessages.CreateCourseCategoryGet),
     InvokePermission(PermissionName.CreateCourseCategory), HttpGet]
    [AdminSearchMarker(Title = AdminSearchTitles.CreateCourseCategory)]
    public async Task<IActionResult> Create()
    {
        ViewData["Categories"] = (await courseCategoryService.GetSelectListAsync()).Value;
        return View();
    }
    
    
    [HttpPost, ValidateAntiForgeryToken]
    [UserActivityLog(Description = ActivityMessages.CreateCourseCategoryPost), InvokePermission(PermissionName.CreateCourseCategory)]
    public async Task<IActionResult> Create(AdminCreateCourseCategoryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Categories"] = (await courseCategoryService.GetSelectListAsync()).Value;
            ShowToasterErrorMessage(ModelState.GetModelErrorsAsString());
            return View(model);
        }
    
        var result = await courseCategoryService.CreateAsync(model);
    
        if (result.IsFailure)
        {
            ViewData["Categories"] = (await courseCategoryService.GetSelectListAsync()).Value;
            ShowToasterErrorMessage(result.Message);
            return View(model);
        }
    
        ShowToasterSuccessMessage(result.Message);
    
        return RedirectToAction(nameof(List));
    }
    
    #endregion
    
    #region Update
    
    [UserActivityLog(Description = ActivityMessages.UpdateCourseCategoryGet), InvokePermission(PermissionName.UpdateCourseCategory),HttpGet]
    [IgnoreUserActivity]
    public async Task<IActionResult> Update(short id)
    {
        var result = await courseCategoryService.FillModelForUpdateAsync(id);
        ViewData["Categories"] = (await courseCategoryService.GetSelectListAsync()).Value;

        
        if (result.IsFailure)
        {           
            ViewData["Categories"] = (await courseCategoryService.GetSelectListAsync()).Value;
            ShowToasterErrorMessage(result.Message);
            return RedirectToAction(nameof(List));
        }
        
        return View(result.Value);
    }
    
    [HttpPost, ValidateAntiForgeryToken]
    [UserActivityLog(Description = ActivityMessages.UpdateCourseCategoryPost), InvokePermission(PermissionName.UpdateCourseCategory)]
    public async Task<IActionResult> Update(AdminUpdateCourseCategoryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Categories"] = (await courseCategoryService.GetSelectListAsync()).Value;
            ShowToasterErrorMessage(ModelState.GetModelErrorsAsString());
            return View(model);
        }
    
        var result = await courseCategoryService.UpdateAsync(model);
    
        if (result.IsFailure)
        {
            ViewData["Categories"] = (await courseCategoryService.GetSelectListAsync()).Value;
            ShowToasterErrorMessage(result.Message);
            return View(model);
        }
    
        ShowToasterSuccessMessage(result.Message);
    
        return RedirectToAction(nameof(List));
    }
    
    #endregion
    
    #region DeleteOrRecover
    
    [UserActivityLog(Description = ActivityMessages.DeleteOrRecoverCourseCategory), InvokePermission(PermissionName.DeleteOrRecoverCourseCategory),HttpGet]
    public async Task<IActionResult> DeleteOrRecover(short id)
    {
        var result = await courseCategoryService.DeleteOrRecoverAsync(id);
        return result.IsSuccess ? Ok(result.Message) : BadRequest(result.Message);
    }
    
    #endregion
    
}