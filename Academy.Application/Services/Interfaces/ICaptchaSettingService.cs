using Academy.Domain.Shared;
using Academy.Domain.ViewModels.CaptchaSetting;

namespace Academy.Application.Services.Interfaces;

public interface ICaptchaSettingService
{
    Task<AdminSideUpdateCaptchaSettingViewModel> FillModelForEditAsync();

    Task<Result> UpdateCaptchaSettingsAsync(AdminSideUpdateCaptchaSettingViewModel  model);
}