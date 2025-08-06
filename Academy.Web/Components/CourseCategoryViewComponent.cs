using Academy.Application.Extensions;
using Academy.Application.Services.Interfaces;
using Academy.Domain.ViewModels.CourseCategory.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Components;


public class CourseCategoryViewComponent(
    ICourseCategoryService courseCategoryService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var result = await courseCategoryService.FilterAsync(new AdminFilterCourseCategoryViewModel());
        return View("CourseCategory", result.Value);
    }
}