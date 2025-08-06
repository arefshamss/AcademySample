using Academy.Domain.Models.Category;
using Academy.Domain.ViewModels.CourseCategory.Admin;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.Application.Mapper.CategoryMappings;

public static class CourseCategoryMapper
{
    public static AdminCourseCategoryViewModel MapToAdminCourseCategoryViewModel(this CourseCategory model) =>
        new()
        {
            Id = model.Id,
            Title = model.Title,
            ParentId = model.ParentId,
            Priority = model.Priority,
            ParentTitle = model.ParentId.HasValue ? model.Parent?.Title : null,
            IsDeleted = model.IsDeleted,
            CreatedDate = model.CreatedDate
        };

    public static CourseCategory MapToCourseCategory(this AdminCreateCourseCategoryViewModel model) =>
        new()
        {
            Title = model.Title.Trim(),
            Slug = model.Slug,
            ParentId = model.ParentId,
            Priority = (short)model.Priority,
        };
    
    public static SelectListItem MapToSelectListItem(this CourseCategory model) =>
        new()
        {
            Text = model.Title,
            Value = model.Id.ToString()
        };
    
    public static AdminUpdateCourseCategoryViewModel MapToAdminUpdateCourseCategoryViewModel(this CourseCategory model) =>
        new()
        {
            Id = model.Id,
            Title = model.Title,
            ParentId = model.ParentId,
            Priority = model.Priority,
            Slug = model.Slug
        };
    
    public static void UpdateCourseCategory(this CourseCategory model, AdminUpdateCourseCategoryViewModel viewModel)
    {
        model.Id = viewModel.Id;
        model.Title = viewModel.Title;
        model.ParentId = viewModel.ParentId;
        model.Priority = (short)viewModel.Priority;
        model.Slug = viewModel.Slug;
    }
}