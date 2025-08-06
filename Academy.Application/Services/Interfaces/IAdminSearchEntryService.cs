using Academy.Domain.ViewModels.AdminSearch;
using Academy.Domain.ViewModels.Common;

namespace Academy.Application.Services.Interfaces;

public interface IAdminSearchEntryService
{
    Task<List<AdminSearchViewModel>> GetAllAsync();
    
    Task SaveEntries(List<AdminSearchViewModel> entries);

    Task<FilterSelect2OptionsViewModel<string>> FilterAsync(FilterSelect2OptionsViewModel<string> filter , int userId);
}