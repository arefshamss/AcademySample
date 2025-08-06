using Academy.Application.Services.Interfaces;
using Academy.Application.Extensions;
using Academy.Application.Mapper.CategoryMappings;
using Academy.Application.Statics;
using Academy.Domain.Contracts;
using Academy.Domain.Enums.Common;
using Academy.Domain.Enums.User;
using Academy.Domain.Models.Category;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.CourseCategory.Admin;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Academy.Application.Services.Implementation;

public class CourseCategoryService(ICourseCategoryRepository courseCategoryRepository):ICourseCategoryService
{
    #region Admin

    #region FilterAsync

    public async Task<Result<AdminFilterCourseCategoryViewModel>> FilterAsync(AdminFilterCourseCategoryViewModel filter)
    {
        filter ??= new();

        var conditions = Filter.GenerateConditions<CourseCategory>();
        
        var orderConditions = Filter.GenerateOrder<CourseCategory>(x => x.Priority);

        #region Filter

        if (!string.IsNullOrWhiteSpace(filter.Title))
            conditions.Add(x => EF.Functions.Like(x.Title, $"%{filter.Title}%"));
        
        switch (filter.DeleteStatus)
        {
            case DeleteStatus.All:
                break;

            case DeleteStatus.Deleted:
                conditions.Add(x => x.IsDeleted);
                break;

            case DeleteStatus.NotDeleted:
                conditions.Add(x => !x.IsDeleted);
                break;
        }

        #endregion

        string[] includes =
        [
            nameof(CourseCategory.Parent)
        ];
        
        await courseCategoryRepository.FilterAsync(filter, conditions, x => x.MapToAdminCourseCategoryViewModel(), orderConditions, includes: includes);
        
        var childCategories = filter.Entities.Where(c => c.ParentId != null).ToList();

        if (childCategories.Any())
        {
            var parentIds = childCategories.Select(c => c.ParentId!.Value).Distinct().ToList();
            
            var parentCategories = await courseCategoryRepository.GetAllAsync(
                x => parentIds.Contains(x.Id) && !x.IsDeleted,
                includes: includes
            );

            var parentViewModels = parentCategories.Select(x => x.MapToAdminCourseCategoryViewModel());
            
            foreach (var parent in parentViewModels)
            {
                if (!filter.Entities.Any(e => e.Id == parent.Id))
                    filter.Entities.Add(parent);
            }
        }
        
        
        return filter;
    }

    #endregion

    #region CreateAsync

    public async Task<Result> CreateAsync(AdminCreateCourseCategoryViewModel model)
    {
        #region Validations

        if (!model.Title.IsNullOrEmptyOrWhiteSpace() && await courseCategoryRepository.AnyAsync(x => x.Title == model.Title && !x.IsDeleted))
            return Result.Failure(string.Format(ErrorMessages.AlreadyExistError, "عنوان"));
        
        if (!model.Slug.IsNullOrEmptyOrWhiteSpace() && await courseCategoryRepository.AnyAsync(x => x.Slug == model.Slug && !x.IsDeleted))
            return Result.Failure(string.Format(ErrorMessages.AlreadyExistError, "اسلاگ"));   
        
        if(model.Priority != null && await courseCategoryRepository.AnyAsync(x => x.Priority == model.Priority && x.ParentId == model.ParentId && !x.IsDeleted ))
            return Result.Failure(string.Format(ErrorMessages.AlreadyExistError, "اولویت نمایش"));   


        #endregion

        var category = model.MapToCourseCategory();
        
        await courseCategoryRepository.InsertAsync(category);
        await courseCategoryRepository.SaveChangesAsync();

        return Result.Success(SuccessMessages.InsertSuccessfullyDone);
    }

    #endregion

    #region GetSelectListAsync
    
    public async Task<Result<SelectList>> GetSelectListAsync() =>   
        new SelectList(await courseCategoryRepository.GetAllAsync(x => x.MapToSelectListItem(), x => !x.IsDeleted && x.ParentId == null), "Value", "Text");

    #endregion
    
    #region FillModelForUpdateAsync

    public async Task<Result<AdminUpdateCourseCategoryViewModel>> FillModelForUpdateAsync(short id)
    {
        if (id < 1)
            return Result.Failure<AdminUpdateCourseCategoryViewModel>(ErrorMessages.BadRequestError);

        string[] includes =
        [
            nameof(CourseCategory.Parent)
        ];

        var category = await courseCategoryRepository.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, includes: includes);

        if (category is null)
            return Result.Failure<AdminUpdateCourseCategoryViewModel>(ErrorMessages.NotFoundError);

        return category.MapToAdminUpdateCourseCategoryViewModel();
    }

    #endregion

    #region UpdateAsync

    public async Task<Result> UpdateAsync(AdminUpdateCourseCategoryViewModel model)
    {
        #region Validations

        if (model.Id < 1)
            return Result.Failure(ErrorMessages.BadRequestError);

        if (!model.Title.IsNullOrEmptyOrWhiteSpace() && await courseCategoryRepository.AnyAsync(x => x.Title == model.Title && !x.IsDeleted && x.Id != model.Id))
            return Result.Failure(string.Format(ErrorMessages.AlreadyExistError, "عنوان"));
        
        if (!model.Slug.IsNullOrEmptyOrWhiteSpace() && await courseCategoryRepository.AnyAsync(x => x.Slug == model.Slug && !x.IsDeleted && x.Id != model.Id))
            return Result.Failure(string.Format(ErrorMessages.AlreadyExistError, "اسلاگ"));   
        
        if(model.Priority != null && await courseCategoryRepository.AnyAsync(x => x.Priority == model.Priority && x.ParentId == model.ParentId && !x.IsDeleted && x.Id != model.Id))
            return Result.Failure(string.Format(ErrorMessages.AlreadyExistError, "اولویت نمایش"));   

        #endregion

        string[] includes =
        [
            nameof(CourseCategory.Parent)
        ];

        var user = await courseCategoryRepository.FirstOrDefaultAsync(x => x.Id == model.Id && !x.IsDeleted, includes: includes);

        if (user is null)
            return Result.Failure(ErrorMessages.NotFoundError);

        user.UpdateCourseCategory(model);

        courseCategoryRepository.Update(user);

        await courseCategoryRepository.SaveChangesAsync();

        return Result.Success(SuccessMessages.UpdateSuccessfullyDone);
    }

    #endregion

    #region DeleteOrRecoverAsync

    public async Task<Result> DeleteOrRecoverAsync(short id)
    {
        if (id < 1)
            return Result.Failure(ErrorMessages.BadRequestError);

        var category = await courseCategoryRepository.GetByIdAsync(id);

        if (category is null)
            return Result.Failure(ErrorMessages.NotFoundError);
        
        if (!category.Title.IsNullOrEmptyOrWhiteSpace() && await courseCategoryRepository.AnyAsync(x => x.Title == category.Title && !x.IsDeleted && x.Id != category.Id))
            return Result.Failure(string.Format(ErrorMessages.ConflictPriorityError, "بازگردانی"));
        
        if (!category.Slug.IsNullOrEmptyOrWhiteSpace() && await courseCategoryRepository.AnyAsync(x => x.Slug == category.Slug && !x.IsDeleted && x.Id != category.Id))
            return Result.Failure(string.Format(ErrorMessages.ConflictPriorityError, "بازگردانی"));
        
        if(await courseCategoryRepository.AnyAsync(x => x.Priority == category.Priority && x.ParentId == category.ParentId && !x.IsDeleted && x.Id != category.Id))
            return Result.Failure(string.Format(ErrorMessages.ConflictPriorityError, "بازگردانی")); 


        var result = courseCategoryRepository.SoftDeleteOrRecover(category);
        await courseCategoryRepository.SaveChangesAsync();


        return result ? Result.Success(SuccessMessages.DeleteSuccess) : Result.Success(SuccessMessages.RecoverSuccess);
    }

    #endregion
    
    #endregion
}