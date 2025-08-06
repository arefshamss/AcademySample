using Academy.Domain.Shared;
using Academy.Domain.ViewModels.SmsSetting;

namespace Academy.Application.Services.Interfaces;

public interface ISmsSettingService
{
    Task<AdminSideUpdateSmsSettingViewModel> FillModelForEditAsync();

    Task<Result> UpdateSmsSettingAsync(AdminSideUpdateSmsSettingViewModel model);

}