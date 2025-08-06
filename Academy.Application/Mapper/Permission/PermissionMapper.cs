using Academy.Domain.ViewModels.Permission;

namespace Academy.Application.Mapper.Permission;

public static class PermissionMapper
{
    public static AdminSidePermissionViewModel ToAdminSidePermissionViewModel(
        this Domain.Models.Permissions.Permission model) =>
        new()
        {
            Id = model.Id,
            DisplayName = model.DisplayName,
            ParentId = model.ParentId
        };
}