using Academy.Application.Cache;
using Academy.Application.Mapper.AdminSearch;
using Academy.Application.Services.Interfaces;
using Academy.Application.Extensions;
using Academy.Domain.Contracts;
using Academy.Domain.Models.AdminSearch;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.AdminSearch;
using Academy.Domain.ViewModels.Common;

namespace Academy.Application.Services.Implementation;

public class AdminSearchEntryService(
    IAdminSearchEntryRepository adminSearchEntryRepository , 
    IMemoryCacheService memoryCacheService , 
    IPermissionRepository permissionRepository,
    IUserRoleMappingRepository userRoleMappingRepository) : IAdminSearchEntryService
{
    public async Task<List<AdminSearchViewModel>> GetAllAsync()
        => await adminSearchEntryRepository.GetAllAsync(AdminSearchMapper.MapSearchEntryToAdminSearchViewModel , default);

    public async Task SaveEntries(List<AdminSearchViewModel> entries)
    {
        var allEntries =  await adminSearchEntryRepository.GetAllAsync();
        adminSearchEntryRepository.DeleteRange(allEntries);
        await adminSearchEntryRepository.SaveChangesAsync();
        
        await adminSearchEntryRepository.InsertRangeAsync(entries.MapToAdminSearchEntries());
        await adminSearchEntryRepository.SaveChangesAsync();
    }

    public async Task<FilterSelect2OptionsViewModel<string>> FilterAsync(FilterSelect2OptionsViewModel<string> filter , int userId)
    {
        var conditions = Filter.GenerateConditions<AdminSearchEntry>();
        var order = Filter.GenerateOrder<AdminSearchEntry>(s => s.Title);
        if(!string.IsNullOrEmpty(filter.Parameter?.Trim()))
            conditions.Add(s => s.Title.Contains(filter.Parameter.Trim().ConvertEnglishToPersianString()));
        var userRoleMappings = await memoryCacheService.GetOrCreateAsync(
            
           CacheKey.Format(CacheKeys.UserRoleMappings, userId),
            async () => await userRoleMappingRepository.GetAllAsync(s => s.UserId == userId));
        
        var rolePermissionMappings = await memoryCacheService.GetOrCreateAsync(CacheKeys.RolePermissionMappings,
            async () => await permissionRepository.GetAllRolePermissions());

        var userPermissions =  rolePermissionMappings.Where(s => userRoleMappings.Any(c => s.RoleId == c.RoleId)).Select(s => s.Permission.UniqueName).ToList();
        
        conditions.Add(item => userPermissions.Contains(item.PermissionName ?? ""));
        
        await adminSearchEntryRepository.FilterAsync(filter, conditions, AdminSearchMapper.MapToAdminSearchViewModel , order);

        return filter;
    }
}