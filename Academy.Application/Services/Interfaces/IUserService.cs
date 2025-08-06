using Academy.Domain.Enums.User;
using Academy.Domain.Models.User;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.User.Account;
using Academy.Domain.ViewModels.User.Admin;
using Academy.Domain.ViewModels.User.Client;

namespace Academy.Application.Services.Interfaces;

public interface IUserService
{
    #region Admin

    Task<Result<AdminFilterUserViewModel>> FilterAsync(AdminFilterUserViewModel filter);

    Task<Result<List<AdminUserExportDataViewModel>>> ExportDataAsync(AdminFilterUserViewModel filter);

    Task<Result<List<AdminUserExportMobileDataViewModel>>> ExportMobileDataAsync(AdminFilterUserViewModel filter);

    Task<Result<AdminUserDetailsViewModel>> GetByIdAsync(int id);

    Task<Result> CreateAsync(AdminCreateUserViewModel model);

    Task<Result<AdminUpdateUserViewModel>> FillModelForUpdateAsync(int id);

    Task<Result> UpdateAsync(AdminUpdateUserViewModel model);

    Task<Result> DeleteOrRecoverAsync(int id);

    #endregion

    #region Account

    Task<Result<AuthenticateUserViewModel>> ConfirmLoginForGoogleAsync(ConfirmLoginForGoogleViewModel model);

    Task<Result<ConfirmLoginViewModel>> ConfirmLoginOrRegisterAsync(LoginOrRegisterViewModel model);

    Task<Result<AuthenticateUserViewModel>> ConfirmOtpCodeAsync(OtpCodeViewModel model);

    Task<Result> ConfirmOtpCodeAsync(int userId, string code, OtpType type);

    Task<Result> SendOtpCodeAsync(string activeCode, string mobileOrEmail);

    Task<Result<ResendOtpCodeViewModel>> ResendOtpCodeAsync(string mobileOrEmail);

    Task<Result<SendForgotPasswordOtpViewModel>> SendForgotPasswordOtpAsync(string mobileOrEmail);

    Task<Result<SendForgotPasswordOtpViewModel>> VerifyForgotPasswordOtpAsync(OtpCodeViewModel model);

    Task<Result> ChangePasswordByResetAsync(ChangePasswordViewModel model);

    Task<Result<string>> GetActiveCodeExpireTimeAsync(string mobileOrEmail);

    #endregion

    #region Client

    Task<Result<ClientUserViewModel>> GetByIdForUserPanelAsync(int id);
    
    Task<Result<ClientUpdateUserViewModel>> FillModelForUserPanelUpdateAsync(int id);
    
    Task<Result> UpdateAsync(ClientUpdateUserViewModel model);
    
    Task<Result<string>> UpdateAvatar(ClientUpdateUserAvatarViewModel model);
    
    Task<Result<string>> DeleteAvatar(int id);
    
    Task<Result> ChangeUserMobile(ClientUpdateMobileViewModel model, int userId);
    
    Task<Result> ChangeUserEmail(ClientUpdateEmailViewModel model, int userId);
    
    Task<Result> GenerateAndSendOtpCodeForUserPanelAsync(int userId, OtpType type, string? mobileOrEmail = null);
    
    Task<Result<string>> GetConfirmationCodeExpireTimeAsync(int userId, OtpType type);

    Task<Result> AddUserPassword(ClientAddPasswordViewModel model, int userId);
    
    Task<Result> ChangeUserPassword(ClientUpdatePasswordViewModel model, int userId);

    #endregion
}