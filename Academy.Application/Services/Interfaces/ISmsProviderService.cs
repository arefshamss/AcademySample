using Academy.Domain.Shared;
using Academy.Domain.ViewModels.SmsProvider;

namespace Academy.Application.Services.Interfaces;

public interface ISmsProviderService
{
    Task<List<AdminSideSmsProviderViewModel>> GetSmsProvidersAsync();

    Task<Result<AdminSideUpdateSmsProviderViewModel>> FillModelForEditAsync(short id);

    Task<Result> UpdateProviderAsync(AdminSideUpdateSmsProviderViewModel model);
}