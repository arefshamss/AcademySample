using Academy.Domain.Enums.Captcha;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.Captcha;

namespace Academy.Application.Services.Interfaces;

public interface ICaptchaService
{
    Task<List<AdminSideCaptchaViewModel>> GetAllCaptchaAsync();
    Task<Result> CreateCaptchaAsync(AdminSideCreateCaptchaViewModel model);
    Task<Result<AdminSideUpdateCaptchaViewModel>> FillModelForEditAsync(short id);
    Task<Result> UpdateCaptchaAsync(AdminSideUpdateCaptchaViewModel model);

    Task<Result<ClientCaptchaViewModel>> GetCaptchaBySectionAsync(CaptchaSection section);

    Task<bool> VerifyCaptchaAsync(CaptchaType type, string? token, string secretKey);

    Task<CaptchaVerificationV2ResponseViewModel> VerifyGoogleV2Async(string token, string secret);

    Task<CaptchaVerificationV3ResponseViewModel> VerifyGoogleV3Async(string token, string secret);

}