using Academy.Application.Services.Interfaces;
using Academy.Application.Mapper.SmsProvider;
using Academy.Domain.Contracts;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.SmsProvider;

namespace Academy.Application.Services.Implementation;

public class SmsProviderService : ISmsProviderService
{
    #region Fields

    private readonly ISmsProviderRepository _providerRepository;

    #endregion

    #region Ctor

    public SmsProviderService(ISmsProviderRepository providerRepository)
    {
        _providerRepository = providerRepository;
    }

    #endregion

    public async Task<List<AdminSideSmsProviderViewModel>> GetSmsProvidersAsync()
    {
        var providers = await _providerRepository.GetAllAsync();
        return providers.Select(s => s.ToAdminSideSmsProviderViewModel()).ToList();
    }

    public async Task<Result<AdminSideUpdateSmsProviderViewModel>> FillModelForEditAsync(short id)
    {
        if (id < 1) return Result.Failure<AdminSideUpdateSmsProviderViewModel>(ErrorMessages.BadRequestError);

        var modelFromDatabase = await _providerRepository.GetByIdAsync(id);

        if (modelFromDatabase is null)
            return Result.Failure<AdminSideUpdateSmsProviderViewModel>(ErrorMessages.NotFoundError);

        return modelFromDatabase.ToAdminSideUpdateSmsProviderViewModel();
    }

    public async Task<Result> UpdateProviderAsync(AdminSideUpdateSmsProviderViewModel model)
    {
        if (model.Id < 1) return Result.Failure(ErrorMessages.BadRequestError);

        var modelFromDatabase = await _providerRepository.GetByIdAsync(model.Id);

        if (modelFromDatabase is null) return Result.Failure(ErrorMessages.BadRequestError);

        model.ToSmsProvider(modelFromDatabase);

        _providerRepository.Update(modelFromDatabase);
        await _providerRepository.SaveChangesAsync();

        return Result.Success(SuccessMessages.UpdateSuccessfullyDone);
    }
}