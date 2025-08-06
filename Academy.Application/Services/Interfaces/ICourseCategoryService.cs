using Academy.Domain.Shared;
using Academy.Domain.ViewModels.CourseCategory.Admin;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.Application.Services.Interfaces;

public interface ICourseCategoryService
{
    Task<Result<AdminFilterCourseCategoryViewModel>> FilterAsync(AdminFilterCourseCategoryViewModel filter);

    Task<Result> CreateAsync(AdminCreateCourseCategoryViewModel model);

    Task<Result<SelectList>> GetSelectListAsync();

    Task<Result<AdminUpdateCourseCategoryViewModel>> FillModelForUpdateAsync(short id);
    
    Task<Result> UpdateAsync(AdminUpdateCourseCategoryViewModel model);

    Task<Result> DeleteOrRecoverAsync(short id);
}