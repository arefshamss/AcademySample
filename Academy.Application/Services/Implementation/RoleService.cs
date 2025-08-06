using Academy.Application.Cache;
using Academy.Application.Services.Interfaces;
using Academy.Application.Mapper.Role;
using Academy.Domain.Contracts;
using Academy.Domain.Enums.Common;
using Academy.Domain.Enums.Role;
using Academy.Domain.Models.Permissions;
using Academy.Domain.Models.Roles;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.Role;
using Microsoft.EntityFrameworkCore;

namespace Academy.Application.Services.Implementation;

public class RoleService(
    IRoleRepository roleRepository,
    IUserRoleMappingRepository userRoleMappingRepository , 
    IMemoryCacheService memoryCacheService) : IRoleService
{
    public async Task<AdminSideFilterRoleViewModel> AdminSideFilterAsync(AdminSideFilterRoleViewModel filter)
    {
        filter ??= new();

        var order = Filter.GenerateOrder<Role>(s => s.CreatedDate, FilterOrderBy.Descending);
        var conditions = Filter.GenerateConditions<Role>();

        #region Filter

        if (!string.IsNullOrEmpty(filter.RoleName))
            conditions.Add(s => EF.Functions.Like(s.RoleName, $"%{filter.RoleName.Trim()}%"));

        switch (filter.RoleSection)
        {
            case FilterRoleSection.All:
                break;
            case FilterRoleSection.Admin:
                conditions.Add(s => s.RoleSection == RoleSection.Admin);
                break;
            case FilterRoleSection.Master:
                conditions.Add(s => s.RoleSection == RoleSection.Master);
                break;
        }

        switch (filter.DeleteStatus)
        {
            case DeleteStatus.NotDeleted:
                conditions.Add(s => !s.IsDeleted);
                break;
            case DeleteStatus.All:
                break;
            case DeleteStatus.Deleted:
                conditions.Add(s => s.IsDeleted);
                break;
        }

        #endregion

        await roleRepository.FilterAsync(filter, conditions, s => s.ToAdminSideRoleViewModel(), order);
        return filter;
    }

    public async Task<Result> CreateRoleAsync(AdminSideCreateRoleViewModel model)
    {
        var role = model.ToRole();
        await roleRepository.InsertAsync(role);
        await roleRepository.SaveChangesAsync();
        await memoryCacheService.RemoveAsync(CacheKeys.RolePermissionMappings);
        return Result.Success(SuccessMessages.InsertSuccessfullyDone);
    }

    public async Task<Result<AdminSideUpdateRoleViewModel>> FillModelForEditAsync(short id)
    {
        if (id < 1) return Result.Failure<AdminSideUpdateRoleViewModel>(ErrorMessages.BadRequestError);

        var modelFromDatabase = await roleRepository.GetByIdAsync(id);

        if (modelFromDatabase is null) return Result.Failure<AdminSideUpdateRoleViewModel>(ErrorMessages.NotFoundError);

        return modelFromDatabase.ToAdminSideUpdateRoleViewModel();
    }

    public async Task<Result> UpdateRoleAsync(AdminSideUpdateRoleViewModel model)
    {
        if (model.Id < 1) return Result.Failure(ErrorMessages.BadRequestError);

        var modelFromDatabase = await roleRepository.GetByIdAsync(model.Id);

        if (modelFromDatabase is null) return Result.Failure(ErrorMessages.NotFoundError);

        model.UpdateRole(modelFromDatabase);

        roleRepository.Update(modelFromDatabase);
        await roleRepository.SaveChangesAsync();
        await memoryCacheService.RemoveAsync(CacheKeys.RolePermissionMappings);
        return Result.Success(SuccessMessages.UpdateSuccessfullyDone);
    }

    public async Task<Result> DeleteOrRecoverRoleAsync(short id)
    {
        if (id < 1) return Result.Failure(ErrorMessages.BadRequestError);

        var modelFromDatabase = await roleRepository.GetByIdAsync(id);

        if (modelFromDatabase is null) return Result.Failure(ErrorMessages.NotFoundError);

        roleRepository.SoftDeleteOrRecover(modelFromDatabase);
        await roleRepository.SaveChangesAsync();
        await memoryCacheService.RemoveAsync(CacheKeys.RolePermissionMappings);
        return Result.Success(SuccessMessages.SuccessfullyDone);
    }

    public async Task<Result> SetPermissionToRole(AdminSideSetPermissionToRoleViewModel model)
    {
        if (model.RoleId < 1) return Result.Failure(ErrorMessages.BadRequestError);

        await roleRepository.DeleteRolePermissionsByIdAsync(model.RoleId);
        var rolePermissionMappings = new List<RolePermissionMapping>();
        if (model.PermissionIds is not null && model.PermissionIds?.Count > 0)
        {
            foreach (var permissionId in model.PermissionIds)
            {
                rolePermissionMappings.Add(new(roleId: model.RoleId, permissionId: permissionId));
            }

            await roleRepository.InsertPermissionsToRoleAsync(rolePermissionMappings);
        }
        await roleRepository.SaveChangesAsync();
        await memoryCacheService.RemoveAsync(CacheKeys.RolePermissionMappings);
        return Result.Success(SuccessMessages.SavedChangesSuccessfully);
    }

    public async Task<List<short>> GetRoleSelectedPermissionsAsync(short roleId) =>
        await roleRepository.GetSelectedPermission(roleId);

    public async Task<bool> IsUserAdminAsync(int userId)
       => await roleRepository.AnyAsync(s =>
            s.RoleSection == RoleSection.Admin && s.UserRoleMappings.Any(item => item.UserId == userId));

    public async Task<bool> IsMasterAsync(int userId)
    {
        return await roleRepository.AnyAsync(s =>
            s.RoleSection == RoleSection.Master &&
            s.UserRoleMappings.Any(item => item.Role.RoleSection==RoleSection.Master && item.UserId == userId));
    }

    #region User Role

    public async Task<string> GetUserRoleIdsSplitByCommaAsync(int userId)
    {
        if (userId < 1) return string.Empty;

        var roleIds = await userRoleMappingRepository.GetAllAsync(s => s.RoleId, s => s.UserId == userId);
        if (roleIds is null) return string.Empty;

        return string.Join(",", roleIds);
    }

    public async Task<Result> SetUserRoleAsync(int userId, string? roleIds)
    {
        if (userId < 1) return Result.Failure(ErrorMessages.BadRequestError);

        await userRoleMappingRepository.ExecuteDeleteRange(s => s.UserId == userId);
        await memoryCacheService.RemoveAsync(CacheKey.Format(CacheKeys.UserRoleMappings , userId));

        List<UserRoleMapping> userRoles = new();

        foreach (var id in roleIds?.Trim().Split(",").ToList() ?? [])
        {
            userRoles.Add(new
                ()
                {
                    UserId = userId,
                    RoleId = Convert.ToInt16(id)
                });
        }

        await userRoleMappingRepository.InsertRangeAsync(userRoles);
        await userRoleMappingRepository.SaveChangesAsync();
        return Result.Success(SuccessMessages.SavedChangesSuccessfully);
    }

    #endregion
}