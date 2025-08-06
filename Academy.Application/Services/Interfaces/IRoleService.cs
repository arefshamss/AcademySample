using Academy.Domain.Shared;
using Academy.Domain.ViewModels.Role;

namespace Academy.Application.Services.Interfaces;

public interface IRoleService
{
    Task<AdminSideFilterRoleViewModel> AdminSideFilterAsync(AdminSideFilterRoleViewModel filter);

    Task<Result> CreateRoleAsync(AdminSideCreateRoleViewModel model);

    Task<Result<AdminSideUpdateRoleViewModel>> FillModelForEditAsync(short id);

    Task<Result> UpdateRoleAsync(AdminSideUpdateRoleViewModel model);

    Task<Result> DeleteOrRecoverRoleAsync(short id);

    Task<Result> SetPermissionToRole(AdminSideSetPermissionToRoleViewModel model);

    Task<List<short>> GetRoleSelectedPermissionsAsync(short roleId);

    Task<bool> IsUserAdminAsync(int userId);

    Task<bool> IsMasterAsync(int userId);

    #region User Role

    Task<string> GetUserRoleIdsSplitByCommaAsync(int userId);

    Task<Result> SetUserRoleAsync(int userId, string? roleIds);

    #endregion
}