using Academy.Application.Cache;
using Academy.Application.Services.Interfaces;
using Academy.Application.Statics;
using Academy.Application.Mapper.Captcha;
using Academy.Domain.Contracts;
using Academy.Domain.DTOs.ArCaptcha.Request;
using Academy.Domain.DTOs.ArCaptcha.Response;
using Academy.Domain.Enums.Captcha;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.Captcha;
using Microsoft.Extensions.Logging;

namespace Academy.Application.Services.Implementation;

public class CaptchaService(
    ICaptchaRepository captchaRepository,
    ICaptchaSettingRepository captchaSettingRepository,
    HttpClient httpClient,
    ILogger<CaptchaService> logger,
    IMemoryCacheService memoryCacheService)
    : ICaptchaService
{
    public async Task<List<AdminSideCaptchaViewModel>> GetAllCaptchaAsync()
    {
        var captcha = await captchaRepository.GetAllAsync();
        return captcha.Select(s => s.ToAdminSideCaptchaViewModel()).ToList();
    }

    public async Task<Result> CreateCaptchaAsync(AdminSideCreateCaptchaViewModel model)
    {
        if (await captchaRepository.AnyAsync(s => s.CaptchaType == model.CaptchaType))
            return Result.Failure(string.Format(ErrorMessages.DuplicatedError, "نوع کپتچا"));

        var captcha = model.ToCaptcha();

        await captchaRepository.InsertAsync(captcha);
        await captchaRepository.SaveChangesAsync();
        await memoryCacheService.RemoveByPrefixAsync(CacheKeys.CaptchaPrefix);
        return Result.Success(SuccessMessages.InsertSuccessfullyDone);
    }

    public async Task<Result<AdminSideUpdateCaptchaViewModel>> FillModelForEditAsync(short id)
    {
        if (id < 1) return Result.Failure<AdminSideUpdateCaptchaViewModel>(ErrorMessages.BadRequestError);

        var model = await captchaRepository.GetByIdAsync(id);

        if (model is null) return Result.Failure<AdminSideUpdateCaptchaViewModel>(ErrorMessages.NotFoundError);

        return model.ToAdminSideUpdateCaptchaViewModel();
    }

    public async Task<Result> UpdateCaptchaAsync(AdminSideUpdateCaptchaViewModel model)
    {
        if (model.Id < 1) return Result.Failure(ErrorMessages.BadRequestError);

        var modelFromDatabase = await captchaRepository.GetByIdAsync(model.Id);

        if (modelFromDatabase is null) return Result.Failure(ErrorMessages.NotFoundError);

        model.ToCaptcha(modelFromDatabase);

        captchaRepository.Update(modelFromDatabase);
        await captchaRepository.SaveChangesAsync();
        await memoryCacheService.RemoveByPrefixAsync(CacheKeys.CaptchaPrefix);
        return Result.Success(SuccessMessages.UpdateSuccessfullyDone);
    }

    public async Task<Result<ClientCaptchaViewModel>> GetCaptchaBySectionAsync(CaptchaSection section)
    {
        return await memoryCacheService.GetOrCreateAsync(CacheKey.Format(CacheKeys.Captcha , section) ,  async () =>
        {
            var captchaSetting = await captchaSettingRepository.FirstOrDefaultAsync(s => s.CaptchaSection == section);

            var captcha =
                await captchaRepository.FirstOrDefaultAsync(s =>
                    s.IsActive && s.CaptchaType == captchaSetting!.CaptchaType);

            return captcha!.ToClientCaptchaViewModel();
        });
    }

    public async Task<bool> VerifyCaptchaAsync(CaptchaType type, string? token, string? secretKey)
    {
        switch (type)
        {
            case CaptchaType.GoogleRecaptchaV2:
                var v2Result = await VerifyGoogleV2Async(token, secretKey);
                return v2Result.Success;
            case CaptchaType.GoogleRecaptchaV3:
                var v3Result = await VerifyGoogleV3Async(token, secretKey);
                return v3Result.Success;
            case CaptchaType.ARCaptcha:
                var captchaSiteKey = await memoryCacheService.GetOrCreateAsync(CacheKeys.ArCaptchaSiteKey ,  async () =>
                {
                    var captcha =
                        await captchaRepository.FirstOrDefaultAsync(s =>
                            s.IsActive && s.CaptchaType == CaptchaType.ARCaptcha);
                    
                    return captcha?.SiteKey ?? "";
                });
                var arResult = await VerifyArCaptchaAsync(new ArCaptchaRequestVerificationDto
                {
                    ChallengeId = token,
                    SiteKey = captchaSiteKey ,
                    SecretKey = secretKey
                });
                return arResult.Success;
        }

        return false;
    }

    public async Task<CaptchaVerificationV2ResponseViewModel> VerifyGoogleV2Async(string token, string secret)
    {
        var response = await httpClient.GetAsync<CaptchaVerificationV2ResponseViewModel>(ApiUrls.GoogleRecaptcha, new
        {
            secret,
            response = token,
        });
        if (!response.IsSuccess)
            logger.LogError("failed to request to google captcha reason : \n " + response.Message);


        return response.Result;
    }

    public async Task<CaptchaVerificationV3ResponseViewModel> VerifyGoogleV3Async(string token, string secret)
    {
        var response = await httpClient.GetAsync<CaptchaVerificationV3ResponseViewModel>(ApiUrls.GoogleRecaptcha, new
        {
            secret,
            response = token,   
        });
        if (!response.IsSuccess)
            logger.LogError("failed to request to google captcha reason : \n " + response.Message);

        return response.Result;
    }

    private async Task<ArCaptchaVerificationResponseDto> VerifyArCaptchaAsync(ArCaptchaRequestVerificationDto requestDto)
    {
        var request = await httpClient.PostAsync<ArCaptchaVerificationResponseDto>(ApiUrls.ArCaptchaVerificationUrl , requestDto);
        if (request.IsSuccess)
            return request.Result;
        return new() { Success = false};
    }
}