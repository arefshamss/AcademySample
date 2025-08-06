namespace Academy.Domain.ViewModels.AdminSearch;

public class AdminSearchViewModel
{
    public required string Title { get; set; }

    public required string Url { get; set; }

    public string? PermissionName { get; set; }
}