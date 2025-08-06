using Academy.Application.Cache;
using Academy.Application.Mapper.EmailSmtp;
using Academy.Application.Services.Interfaces;
using Academy.Application.Extensions;
using Academy.Domain.Contracts;
using Academy.Domain.Enums.Common;
using Academy.Domain.Enums.EmailSmtp;
using Academy.Domain.Models.EmailSmtp;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.EmailSmtp;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Academy.Application.Services.Implementation;

public class EmailSmtpService(
    IEmailSmtpRepository emailSmtpRepository,
    ISiteSettingRepository siteSettingRepository,
    IMemoryCacheService memoryCacheService)
    : IEmailSmtpService
{
    public async Task<FilterAdminSideEmailSmtpViewModel> FilterAsync(FilterAdminSideEmailSmtpViewModel filter)
    {
        filter ??= new();

        var conditions = Filter.GenerateConditions<EmailSmtp>();
        var order = Filter.GenerateOrder<EmailSmtp>(s => s.CreatedDate, FilterOrderBy.Descending);

        #region Filter

        if (!string.IsNullOrEmpty(filter.EmailAddress))
            conditions.Add(s => EF.Functions.Like(s.EamilAddress, $"%{filter.EmailAddress.Trim()}%"));

        switch (filter.DeleteStatus)
        {
            case DeleteStatus.NotDeleted:
                conditions.Add(s => !s.IsDeleted);
                break;
            case DeleteStatus.All:
                break;
            case DeleteStatus.Deleted:
                conditions.Add(s => s.IsDeleted);
                break;
        }

        #endregion

        await emailSmtpRepository.FilterAsync(filter, conditions, s => s.ToAdminSideEmailSmtpViewModel(), order);

        return filter;
    }

    public async Task<Result> CreateAsync(AdminSideCreateEmailSmtpViewModel model)
    {
        var emailSmtp = model.ToEmailSmtp();

        await emailSmtpRepository.InsertAsync(emailSmtp);
        await emailSmtpRepository.SaveChangesAsync();
        return Result.Success(SuccessMessages.InsertSuccessfullyDone);
    }

    public async Task<Result<AdminSideUpdateEmailSmtpViewModel>> FillModelForEditAsync(short id)
    {
        if (id < 1) return Result.Failure<AdminSideUpdateEmailSmtpViewModel>(ErrorMessages.BadRequestError);

        var modelFromDatabase = await emailSmtpRepository.GetByIdAsync(id);

        if (modelFromDatabase is null)
            return Result.Failure<AdminSideUpdateEmailSmtpViewModel>(ErrorMessages.NotFoundError);

        return modelFromDatabase.ToUpdateAdminSideEmailSmtpViewModel();
    }

    public async Task<Result> UpdateAsync(AdminSideUpdateEmailSmtpViewModel model)
    {
        if (model.Id < 1) return Result.Failure(ErrorMessages.NotFoundError);

        var modelFromDatabase = await emailSmtpRepository.GetByIdAsync(model.Id);

        if (modelFromDatabase is null)
            return Result.Failure(ErrorMessages.NotFoundError);

        model.ToEmailSmtp(modelFromDatabase);

        emailSmtpRepository.Update(modelFromDatabase);
        await emailSmtpRepository.SaveChangesAsync();

        await memoryCacheService.RemoveByPrefixAsync(CacheKeys.SmtpEmailPrefix);
        return Result.Success(SuccessMessages.UpdateSuccessfullyDone);
    }

    public async Task<Result> SoftDeleteOrRecoverAsync(short id)
    {
        if (id < 1)
            return Result.Failure(ErrorMessages.BadRequestError);

        var productAttribute = await emailSmtpRepository.GetByIdAsync(id);

        if (productAttribute is null)
            return Result.Failure(ErrorMessages.BadRequestError);

        emailSmtpRepository.SoftDeleteOrRecover(productAttribute);
        await emailSmtpRepository.SaveChangesAsync();

        return Result.Success(SuccessMessages.SuccessfullyDone);
    }

    public async Task<Result<EmailSmtpViewModel>> GetDefaultSimpleEmailSmtpAsync()
    {
        var smtpId = (await siteSettingRepository.FirstOrDefaultAsync())!.DefaultSimpleSmtpId;

        if (!smtpId.HasValue)
            return null;

        return await memoryCacheService.GetOrCreateAsync(CacheKeys.DefaultSimpleSmtpId, async () =>
        {
            var defaultSimpleSmtp = await emailSmtpRepository.GetByIdAsync(smtpId.Value);
            return defaultSimpleSmtp!.ToEmailSmtpViewModel();
        });
    }

    public async Task<Result<EmailSmtpViewModel>> GetDefaultMailServerEmailSmtpAsync()
    {
        var smtpId = (await siteSettingRepository.FirstOrDefaultAsync())!.DefaultMailServerSmtpId;

        if (!smtpId.HasValue)
            return null;

        return await memoryCacheService.GetOrCreateAsync(CacheKeys.DefaultMailServerSmtpId, async () =>
        {
            var defaultSimpleSmtp = await emailSmtpRepository.GetByIdAsync(smtpId.Value);
            return defaultSimpleSmtp!.ToEmailSmtpViewModel();
        });
    }

    public async Task<Result<SelectList>> GetForSelectList(EmailSmtpType smtpType, short? selectedValue = null)
    {
        var items = await emailSmtpRepository.GetAllAsync(
            EmailSmtpMapper.MapToSelectViewModel
            , s =>
                s.EmailSmtpType == smtpType
                && !s.IsDeleted);

        return Result.Success(items.ToSelectList(selectedValue ?? default));
    }
}