using Academy.Application.Services.Interfaces;
using Academy.Application.Mapper.SmsSetting;
using Academy.Domain.Contracts;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.SmsSetting;

namespace Academy.Application.Services.Implementation;

public class SmsSettingService : ISmsSettingService
{
    #region Fields

    private readonly ISmsSettingRepository _smsSettingRepository;

    #endregion

    #region Ctor

    public SmsSettingService(ISmsSettingRepository smsSettingRepository)
    {
        _smsSettingRepository = smsSettingRepository;
    }

    #endregion


    public async Task<AdminSideUpdateSmsSettingViewModel> FillModelForEditAsync()
    {
        var smsSettings = await _smsSettingRepository.GetAllAsync();
        return new AdminSideUpdateSmsSettingViewModel
        {
            SmsSettings = smsSettings.Select(s => s.ToAdminSideSmsSettingViewModel()).ToList()
        };
    }

    public async Task<Result> UpdateSmsSettingAsync(AdminSideUpdateSmsSettingViewModel model)
    {
        foreach (var smsSection in model.SmsSettings)
        {
            var smsSetting = await _smsSettingRepository.FirstOrDefaultAsync(s => s.SmsSettingSection == smsSection.SmsSettingSection);

            if (smsSetting is null)
            {
                smsSetting = smsSection.ToSmsSetting();
                await _smsSettingRepository.InsertAsync(smsSetting);
            }
            else
            {
                smsSetting.SmsProviderType = smsSection.SmsProviderType;
                _smsSettingRepository.Update(smsSetting);
            }
        }
        await _smsSettingRepository.SaveChangesAsync();
        return Result.Success(SuccessMessages.SuccessfullyDone);
    }
}