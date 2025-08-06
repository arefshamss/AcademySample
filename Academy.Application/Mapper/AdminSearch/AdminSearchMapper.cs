using System.Linq.Expressions;
using Academy.Domain.Models.AdminSearch;
using Academy.Domain.ViewModels.AdminSearch;
using Academy.Domain.ViewModels.Common;

namespace Academy.Application.Mapper.AdminSearch;

internal static class AdminSearchMapper
{
    internal static Expression<Func<AdminSearchEntry, AdminSearchViewModel>> MapSearchEntryToAdminSearchViewModel => item => new()
    {
        Title = item.Title,
        Url = item.Url,
        PermissionName = item.PermissionName,
    };
    
    internal static List<AdminSearchEntry> MapToAdminSearchEntries(
        this List<AdminSearchViewModel> adminSearchViewModels) =>
        adminSearchViewModels.Select(item => new AdminSearchEntry
        {
            Title = item.Title,
            Url = item.Url,
            PermissionName = item.PermissionName,
        }).ToList();

    internal static Expression<Func<AdminSearchEntry, Select2OptionViewModel<string>>> MapToAdminSearchViewModel =>
        item => new()
        {
            Text = item.Title,
            Id = item.Url,
        };
}