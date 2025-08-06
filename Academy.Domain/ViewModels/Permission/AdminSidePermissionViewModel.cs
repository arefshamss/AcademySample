namespace Academy.Domain.ViewModels.Permission;

public class AdminSidePermissionViewModel
{
    public short Id { get; set; }

    public short? ParentId { get; set; }

    public string DisplayName { get; set; }

}