using Academy.Application.Extensions;
using Academy.Application.Services.Interfaces;
using Academy.Application.Statics;
using Academy.Application.Tools;
using Academy.Application.DTOs;
using Academy.Application.Mapper.UserMappings;
using Academy.Domain.Contracts;
using Academy.Domain.Enums.Common;
using Academy.Domain.Enums.SmsSetting;
using Academy.Domain.Enums.User;
using Academy.Domain.Models.User;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.User.Account;
using Academy.Domain.ViewModels.User.Admin;
using Academy.Domain.ViewModels.User.Client;
using Microsoft.EntityFrameworkCore;

namespace Academy.Application.Services.Implementation;

public class UserService(
    IUserRepository userRepository,
    IUserInformationRepository userInformationRepository,
    ISmsSenderService smsService,
    IEmailSenderService emailService)
    : IUserService
{
    #region Admin

    #region FilterAsync

    public async Task<Result<AdminFilterUserViewModel>> FilterAsync(AdminFilterUserViewModel filter)
    {
        filter ??= new();

        var conditions = Filter.GenerateConditions<User>();
        var orderConditions = Filter.GenerateOrder<User>(x => x.CreatedDate, FilterOrderBy.Descending);

        #region Filter

        if (!string.IsNullOrWhiteSpace(filter.FullName))
            conditions.Add(x =>
                EF.Functions.Like(x.FirstName, $"%{filter.FullName}%") ||
                EF.Functions.Like(x.LastName, $"%{filter.FullName}%"));

        if (!string.IsNullOrWhiteSpace(filter.Mobile))
            conditions.Add(x => EF.Functions.Like(x.Mobile, $"%{filter.Mobile}%"));

        if (!string.IsNullOrWhiteSpace(filter.Email))
            conditions.Add(x => EF.Functions.Like(x.Email, $"%{filter.Email}%"));

        if (!filter.FromDate.IsNullOrEmptyOrWhiteSpace())
            conditions.Add(s => s.CreatedDate >= filter.FromDate!.ToMiladiDateTime());

        if (!filter.ToDate.IsNullOrEmptyOrWhiteSpace())
            conditions.Add(s => s.CreatedDate <= filter.ToDate!.ToMiladiDateTime());

        switch (filter.DeleteStatus)
        {
            case DeleteStatus.All:
                break;

            case DeleteStatus.Deleted:
                conditions.Add(x => x.IsDeleted);
                break;

            case DeleteStatus.NotDeleted:
                conditions.Add(x => !x.IsDeleted);
                break;
        }

        switch (filter.UserStatus)
        {
            case FilterUserStatus.All:
                break;

            case FilterUserStatus.Active:
                conditions.Add(x => !x.IsBanned);
                break;

            case FilterUserStatus.Banned:
                conditions.Add(x => x.IsBanned);
                break;
        }

        switch (filter.UserType)
        {
            case FilterUserType.All:
                break;

            //TODO: Filter Not Added Yet!
        }

        switch (filter.UserMobileStatus)
        {
            case FilterUserMobileStatus.All:
                break;

            case FilterUserMobileStatus.Active:
                conditions.Add(x => !x.IsMobileActive);
                break;

            case FilterUserMobileStatus.NotActive:
                conditions.Add(x => x.IsMobileActive);
                break;
        }

        switch (filter.UserEmailStatus)
        {
            case FilterUserEmailStatus.All:
                break;

            case FilterUserEmailStatus.Active:
                conditions.Add(x => !x.IsEmailActive);
                break;

            case FilterUserEmailStatus.NotActive:
                conditions.Add(x => x.IsEmailActive);
                break;
        }

        switch (filter.UserTicketStatus)
        {
            case FilterUserTicketStatus.All:
                break;

            case FilterUserTicketStatus.Active:
                conditions.Add(x => !x.IsBannedFromTicket);
                break;

            case FilterUserTicketStatus.Banned:
                conditions.Add(x => x.IsBannedFromTicket);
                break;
        }

        switch (filter.UserCommentStatus)
        {
            case FilterUserCommentStatus.All:
                break;

            case FilterUserCommentStatus.Active:
                conditions.Add(x => !x.IsBannedFromComment);
                break;

            case FilterUserCommentStatus.Banned:
                conditions.Add(x => x.IsBannedFromComment);
                break;
        }

        #endregion

        string[] includes =
        [
            nameof(User.UserInformation)
        ];

        await userRepository.FilterAsync(filter, conditions, x => x.MapToAdminUserViewModel(), orderConditions,
            includes: includes);
        return filter;
    }

    #endregion

    #region ExportDataAsync

    public async Task<Result<List<AdminUserExportDataViewModel>>> ExportDataAsync(AdminFilterUserViewModel filter)
    {
        var result = await FilterAsync(filter);
        if (!result.IsSuccess)
            return Result.Failure<List<AdminUserExportDataViewModel>>(result.Message);

        var exportData = result.Value.Entities
            .Select(x => x.MapToAdminUserExportDataViewModel())
            .ToList();

        return Result.Success(exportData);
    }

    public async Task<Result<List<AdminUserExportMobileDataViewModel>>> ExportMobileDataAsync(
        AdminFilterUserViewModel filter)
    {
        var result = await FilterAsync(filter);
        if (!result.IsSuccess)
            return Result.Failure<List<AdminUserExportMobileDataViewModel>>(result.Message);

        var exportData = result.Value.Entities
            .Select(x => x.MapToAdminUserExportMobileDataViewModel())
            .ToList();

        return Result.Success(exportData);
    }

    #endregion

    #region GetByIdAsync

    public async Task<Result<AdminUserDetailsViewModel>> GetByIdAsync(int id)
    {
        if (id < 1)
            return Result.Failure<AdminUserDetailsViewModel>(ErrorMessages.BadRequestError);

        string[] includes =
        [
            nameof(User.UserInformation)
        ];
        var user = await userRepository.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, includes: includes);

        if (user is null)
            return Result.Failure<AdminUserDetailsViewModel>(ErrorMessages.NotFoundError);

        return user.MapToAdminUserDetailsViewModel();
    }

    #endregion

    #region CreateAsync

    public async Task<Result> CreateAsync(AdminCreateUserViewModel model)
    {
        #region Validations

        if (model.Mobile.IsNullOrEmptyOrWhiteSpace() && model.Email.IsNullOrEmptyOrWhiteSpace())
            return Result.Failure(ErrorMessages.RequiredMobileOrEmail);

        if (!model.Mobile.IsNullOrEmptyOrWhiteSpace() &&
            await userRepository.AnyAsync(x => x.Mobile == model.Mobile && !x.IsDeleted))
            return Result.Failure(string.Format(ErrorMessages.AlreadyExistError, "تلفن همراه"));

        if (!model.Email.IsNullOrEmptyOrWhiteSpace() &&
            await userRepository.AnyAsync(x => x.Email == model.Email && !x.IsDeleted))
            return Result.Failure(string.Format(ErrorMessages.AlreadyExistError, "ایمیل"));

        if (!model.NationalCode.IsNullOrEmptyOrWhiteSpace() && await userRepository.AnyAsync(x =>
                x.UserInformation.NationalCode == model.NationalCode && !x.IsDeleted))
            return Result.Failure(string.Format(ErrorMessages.AlreadyExistError, "کد ملی"));

        #endregion

        var user = model.MapToUser();

        if (!user.IsEmailActive && !user.IsMobileActive)
        {
            user.IsBanned = true;
            user.IsBannedFromTicket = true;
            user.IsBannedFromComment = true;
        }

        #region Add User Avatar

        if (model.AvatarImageFile is not null)
        {
            var result =
                await model.AvatarImageFile.AddImageToServer(FilePaths.UserImagePath, 300, 300,
                    FilePaths.UserImageThumbPath);

            if (result.IsFailure)
                return Result.Failure(result.Message!);

            user.AvatarImageName = result.Value;
        }
        else
        {
            user.AvatarImageName = SiteTools.DefaultImageName;
        }

        #endregion

        await userRepository.InsertAsync(user);
        await userRepository.SaveChangesAsync();

        return Result.Success(SuccessMessages.InsertSuccessfullyDone);
    }

    #endregion

    #region FillModelForUpdateAsync

    public async Task<Result<AdminUpdateUserViewModel>> FillModelForUpdateAsync(int id)
    {
        if (id < 1)
            return Result.Failure<AdminUpdateUserViewModel>(ErrorMessages.BadRequestError);

        string[] includes =
        [
            nameof(User.UserInformation)
        ];

        var user = await userRepository.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, includes: includes);

        if (user is null)
            return Result.Failure<AdminUpdateUserViewModel>(ErrorMessages.NotFoundError);

        return user.MapToAdminUpdateUserViewModel();
    }

    #endregion

    #region UpdateAsync

    public async Task<Result> UpdateAsync(AdminUpdateUserViewModel model)
    {
        #region Validations

        if (model.Id < 1)
            return Result.Failure(ErrorMessages.BadRequestError);

        if (await userRepository.AnyAsync(x =>
                x.Mobile == model.Mobile && x.Id != model.Id && !x.IsDeleted &&
                !model.Mobile.IsNullOrEmptyOrWhiteSpace()))
            return Result.Failure(string.Format(ErrorMessages.ConflictActiveUserError, "ویرایش"));


        if (await userRepository.AnyAsync(x =>
                x.Email == model.Email && x.Id != model.Id && !x.IsDeleted && !model.Email.IsNullOrEmptyOrWhiteSpace()))
            return Result.Failure(string.Format(ErrorMessages.ConflictActiveUserError, "ویرایش"));


        if (!model.NationalCode.IsNullOrEmptyOrWhiteSpace() && await userRepository.AnyAsync(x =>
                x.UserInformation.NationalCode == model.NationalCode && x.Id != model.Id && !x.IsDeleted &&
                !model.NationalCode.IsNullOrEmptyOrWhiteSpace()))
            return Result.Failure(string.Format(ErrorMessages.AlreadyExistError, "کد ملی"));

        #endregion

        string[] includes =
        [
            nameof(User.UserInformation)
        ];

        var user = await userRepository.FirstOrDefaultAsync(x => x.Id == model.Id && !x.IsDeleted, includes: includes);

        if (user is null)
            return Result.Failure(ErrorMessages.NotFoundError);

        user.UpdateUser(model);

        if (!user.IsEmailActive && !user.IsMobileActive)
        {
            user.IsBanned = true;
        }

        if (user.IsBanned)
        {
            user.IsBannedFromTicket = true;
            user.IsBannedFromComment = true;
        }

        if (!model.Password.IsNullOrEmptyOrWhiteSpace())
            user.Password = model.Password!.EncodePasswordSHA512();

        #region Add User Avatar

        if (model.AvatarImageFile is not null)
        {
            var result = await model.AvatarImageFile.AddImageToServer(FilePaths.UserImagePath, 300, 300,
                FilePaths.UserImageThumbPath, deleteFileName: user.AvatarImageName);

            if (result.IsFailure)
                return Result.Failure(result.Message!);

            user.AvatarImageName = result.Value;
        }

        #endregion

        userRepository.Update(user);

        await userRepository.SaveChangesAsync();

        return Result.Success(SuccessMessages.UpdateSuccessfullyDone);
    }

    #endregion

    #region DeleteOrRecoverAsync

    public async Task<Result> DeleteOrRecoverAsync(int id)
    {
        if (id < 1)
            return Result.Failure(ErrorMessages.BadRequestError);

        var user = await userRepository.GetByIdAsync(id);

        if (user is null)
            return Result.Failure(ErrorMessages.NotFoundError);

        if (!user.Mobile.IsNullOrEmptyOrWhiteSpace() &&
            await userRepository.AnyAsync(x => x.Mobile == user.Mobile && x.Id != user.Id && !x.IsDeleted))
            return Result.Failure(string.Format(ErrorMessages.ConflictUserChangeError, "بازگردانی"));

        if (!user.Email.IsNullOrEmptyOrWhiteSpace() &&
            await userRepository.AnyAsync(x => x.Email == user.Email && x.Id != user.Id && !x.IsDeleted))
            return Result.Failure(string.Format(ErrorMessages.ConflictUserChangeError, "بازگردانی"));

        if (user.UserInformation != null && !user.UserInformation.NationalCode.IsNullOrEmptyOrWhiteSpace() &&
            await userRepository.AnyAsync(x =>
                x.UserInformation != null && x.UserInformation.NationalCode == user.UserInformation.NationalCode &&
                x.Id != user.Id && !x.IsDeleted))
            return Result.Failure(string.Format(ErrorMessages.ConflictUserChangeError, "بازگردانی"));

        var result = userRepository.SoftDeleteOrRecover(user);
        await userRepository.SaveChangesAsync();


        return result ? Result.Success(SuccessMessages.DeleteSuccess) : Result.Success(SuccessMessages.RecoverSuccess);
    }

    #endregion

    #endregion

    #region Account

    #region ConfirmLoginForGoogleAsync

    public async Task<Result<AuthenticateUserViewModel>> ConfirmLoginForGoogleAsync(
        ConfirmLoginForGoogleViewModel model)
    {
        var user = await userRepository.FirstOrDefaultAsync(s =>
            (s.GoogleAuthenticationId != null && s.GoogleAuthenticationId == model.GoogleAuthenticationId) ||
            s.Email != null && s.Email.Trim() == model.Email);

        if (user is not null)
        {
            if (user.IsDeleted)
                return Result.Failure<AuthenticateUserViewModel>(ErrorMessages.UserDeletedError);

            if (user.IsBanned)
                return Result.Failure<AuthenticateUserViewModel>(ErrorMessages.UserBannedError);

            if (!string.IsNullOrEmpty(user.Email) && user.Email.Trim() == model.Email)
            {
                if (string.IsNullOrEmpty(user.FirstName) && string.IsNullOrEmpty(user.LastName))
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                }

                user.GoogleAuthenticationId = model.GoogleAuthenticationId;
                userRepository.Update(user);
                await userRepository.SaveChangesAsync();
            }

            return user.MapToAuthenticateUserViewModel();
        }

        User newUser = new()
        {
            GoogleAuthenticationId = model.GoogleAuthenticationId,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Mobile = model.Mobile,
            IsEmailActive = true,
            Email = model.Email,
            AvatarImageName = SiteTools.DefaultImageName,
            IsBanned = false,
            Password = "Google-" + Guid.NewGuid()
        };


        await userRepository.InsertAsync(newUser);
        await userRepository.SaveChangesAsync();
        return newUser.MapToAuthenticateUserViewModel();
    }

    #endregion

    #region ConfirmLoginOrRegisterAsync

    public async Task<Result<ConfirmLoginViewModel>> ConfirmLoginOrRegisterAsync(LoginOrRegisterViewModel model)
    {
        if (model.IsLoginByPassword && model.Password.IsNullOrEmptyOrWhiteSpace())
            return Result.Failure<ConfirmLoginViewModel>(string.Format(ErrorMessages.RequiredError, "رمز عبور"));

        string input = model.MobileOrEmail.Trim();

        bool isEmail = UserRegex.EmailRegex().IsMatch(input);
        bool isMobile = UserRegex.MobileRegex().IsMatch(input);

        if (!isEmail && !isMobile)
            return Result.Failure<ConfirmLoginViewModel>(ErrorMessages.NotValidMobileOrEmail);


        var user = await userRepository.FirstOrDefaultAsync(x =>
            x.Mobile == input && !x.IsDeleted || x.Email == input && !x.IsDeleted);

        string successMessage;

        if (model.IsLoginByPassword)
        {
            if (user is null)
                return Result.Failure<ConfirmLoginViewModel>(ErrorMessages.UserNotFoundError);

            if (!user.IsEmailActive && !user.IsMobileActive)
                return Result.Failure<ConfirmLoginViewModel>(ErrorMessages.UserNotActiveError);

            string hashedPassword = model.Password.EncodePasswordSHA512();

            if (user.Password != hashedPassword)
                return Result.Failure<ConfirmLoginViewModel>(ErrorMessages.PasswordNotCorrect);

            successMessage = SuccessMessages.LoginSuccessfullyDone;
        }
        else
        {
            if (user is null)
            {
                if (await userRepository.AnyAsync(x => x.Email == input && !x.IsDeleted))
                    return Result.Failure<ConfirmLoginViewModel>(string.Format(ErrorMessages.ConflictError, "ایمیل"));

                if (await userRepository.AnyAsync(x => x.Mobile == input && !x.IsDeleted))
                    return Result.Failure<ConfirmLoginViewModel>(string.Format(ErrorMessages.ConflictError,
                        "شماره همراه"));

                user = model.MapToUser(isEmail);

                user.AvatarImageName = SiteTools.DefaultImageName;

                if (isEmail)
                    user.IsEmailActive = true;
                else
                    user.IsMobileActive = true;

                await userRepository.InsertAsync(user);
                await userRepository.SaveChangesAsync();
            }

            if (!user.IsEmailActive && !user.IsMobileActive)
                return Result.Failure<ConfirmLoginViewModel>(ErrorMessages.UserNotActiveError);

            SetUserActiveCode(user, OtpType.Default);

            userRepository.Update(user);
            await userRepository.SaveChangesAsync();

            successMessage = isEmail
                ? SuccessMessages.OtpCodeSentSuccessfullyToEmail
                : SuccessMessages.OtpCodeSentSuccessfullyToMobile;
        }

        return Result.Success(user.MapToConfirmLoginViewModel(model.IsLoginByPassword, isEmail), successMessage);
    }

    #endregion

    #region ConfirmOtpCodeAsync

    public async Task<Result<AuthenticateUserViewModel>> ConfirmOtpCodeAsync(OtpCodeViewModel model)
    {
        if (model.UserId < 1)
            return Result.Failure<AuthenticateUserViewModel>(ErrorMessages.SomethingWentWrong);

        string input = model.MobileOrEmail.Trim();

        bool isEmail = UserRegex.EmailRegex().IsMatch(input);
        bool isMobile = UserRegex.MobileRegex().IsMatch(input);

        var user = await userRepository.FirstOrDefaultAsync(s => s.Id == model.UserId && !s.IsDeleted);

        if (user is null)
            return Result.Failure<AuthenticateUserViewModel>(ErrorMessages.BadRequestError);

        if (user.ActiveCodeExpireTime < DateTime.Now)
            return Result.Failure<AuthenticateUserViewModel>(ErrorMessages.ExpireConfirmCodeError);

        if (user.ActiveCode is null || user.ActiveCode != model.Code)
            return Result.Failure<AuthenticateUserViewModel>(ErrorMessages.InvalidConfirmationCode);

        user.ActiveCode = null;
        user.ActiveCodeExpireTime = null;

        if (isEmail)
        {
            if (!user.IsEmailActive)
                user.IsEmailActive = true;
        }

        if (isMobile)
        {
            if (!user.IsMobileActive)
                user.IsMobileActive = true;
        }


        userRepository.Update(user);
        await userRepository.SaveChangesAsync();

        return user.MapToAuthenticateUserViewModel();
    }

    #endregion

    #region SendOtpCodeAsync

    public async Task<Result> SendOtpCodeAsync(string activeCode, string mobileOrEmail)
    {
        // string input = mobileOrEmail.Trim();
        //
        // if (string.IsNullOrWhiteSpace(input))
        //     return Result.Failure(ErrorMessages.RequiredMobileOrEmail);
        //
        // bool isEmail = UserRegex.EmailRegex().IsMatch(input);
        // bool isMobile = UserRegex.MobileRegex().IsMatch(input);
        //
        // if (!isEmail && !isMobile)
        //     return Result.Failure(ErrorMessages.NotValidMobileOrEmail);
        //
        // if (isEmail)
        // {
        //     string body = $"<p>کد تایید شما: <strong>{activeCode}</strong></p>";
        //
        //     var newMail = new SendMailRequestDto
        //     {
        //         Body = body,
        //         Subject = "تایید حساب کاربری",
        //         To = input,
        //         Title = "کد تایید",
        //         From = "آکادمی"
        //     };
        //
        //     var emailResult = await emailService.SendAsync(newMail);
        //
        //     return emailResult.IsSuccess
        //         ? Result.Success(SuccessMessages.OtpCodeSentSuccessfully)
        //         : Result.Failure(message: ErrorMessages.SmsDidNotSendError);
        // }
        //
        // string message = $"کد تایید شما جهت ورود به آکادمی: {activeCode}";
        // Result smsResult;
        // byte retryCount = 0;
        // do
        // {
        //     smsResult = await smsService.SendSmsAsync(input, message, SmsSettingSection.Account);
        //     retryCount++;
        // } while (smsResult.IsFailure && retryCount <= 5);
        //
        // return smsResult.IsSuccess
        //     ? Result.Success(SuccessMessages.OtpCodeSentSuccessfullyToMobile)
        //     : Result.Failure(message: ErrorMessages.SmsDidNotSendError);


        return Result.Success(SuccessMessages.OtpCodeSentSuccessfullyToMobile);
    }

    #endregion

    #region ResendOtpCodeSmsAsync

    public async Task<Result<ResendOtpCodeViewModel>> ResendOtpCodeAsync(string mobileOrEmail)
    {
        string input = mobileOrEmail.Trim() ?? "";

        bool isEmail = UserRegex.EmailRegex().IsMatch(input);

        var user = await userRepository.FirstOrDefaultAsync(x => x.Mobile == input || x.Email == input);

        if (user is null) return Result.Failure<ResendOtpCodeViewModel>(ErrorMessages.UserNotFoundError);

        if (string.IsNullOrEmpty(user.Mobile) && string.IsNullOrEmpty(user.Email))
            return Result.Failure<ResendOtpCodeViewModel>(ErrorMessages.SomethingWentWrong);

        if (DateTime.Now < user.ActiveCodeExpireTime)
            return Result.Failure<ResendOtpCodeViewModel>(ErrorMessages.ActiveCodeExpireDateTime);

        SetUserActiveCode(user, OtpType.Default);

        userRepository.Update(user);

        await userRepository.SaveChangesAsync();

        if (user.Mobile is null && user.Email is not null)
            return (await SendOtpCodeAsync(user.ActiveCode, user.Email))
                .IsSuccess
                    ? Result.Success(user.MapToResendOtpCodeViewModel(), SuccessMessages.OtpCodeSentSuccessfullyToEmail)
                    : Result.Failure<ResendOtpCodeViewModel>(ErrorMessages.EmailDidNotSendError);

        return (await SendOtpCodeAsync(user.ActiveCode, user.Mobile))
            .IsSuccess
                ? Result.Success(user.MapToResendOtpCodeViewModel(), SuccessMessages.OtpCodeSentSuccessfullyToMobile)
                : Result.Failure<ResendOtpCodeViewModel>(ErrorMessages.SmsDidNotSendError);
    }

    #endregion

    #region GetActiveCodeExpireTimeAsync
    
    public async Task<Result<string>> GetActiveCodeExpireTimeAsync(string mobileOrEmail) =>
        (await userRepository.FirstOrDefaultAsync(x =>
            (x.Mobile == mobileOrEmail || x.Email == mobileOrEmail) && !x.IsDeleted))!.ActiveCodeExpireTime!.Value
        .ToJavaScriptDateTimeStandard();

    #endregion

    #region SendForgotPasswordOtpAsync
    
    public async Task<Result<SendForgotPasswordOtpViewModel>> SendForgotPasswordOtpAsync(string mobileOrEmail)
    {
        string input = mobileOrEmail?.Trim() ?? "";
        
        if (string.IsNullOrWhiteSpace(input))
            return Result.Failure<SendForgotPasswordOtpViewModel>(ErrorMessages.RequiredMobileOrEmail);
    
        bool isEmail = UserRegex.EmailRegex().IsMatch(input);
        bool isMobile = UserRegex.MobileRegex().IsMatch(input);
    
        if (!isEmail && !isMobile)
            return Result.Failure<SendForgotPasswordOtpViewModel>(ErrorMessages.NotValidMobileOrEmail);
    
        var user = await userRepository.FirstOrDefaultAsync(x =>
            (x.Email == input && !x.IsDeleted || x.Mobile == input) && !x.IsDeleted);
    
        if (user is null)
            return Result.Failure<SendForgotPasswordOtpViewModel>(ErrorMessages.UserNotFoundError);
    
        if (!user.IsEmailActive && !user.IsMobileActive)
            return Result.Failure<SendForgotPasswordOtpViewModel>(ErrorMessages.UserNotActiveError);
        
        SetUserActiveCode(user, OtpType.Default);
        
        userRepository.Update(user);
        await userRepository.SaveChangesAsync();

        if (user.Mobile is null && user.Email is not null)
            return (await SendOtpCodeAsync(user.ActiveCode, user.Email))
                .IsSuccess
                    ? Result.Success(user.MapToSendForgotPasswordOtpViewModel(), SuccessMessages.OtpCodeSentSuccessfullyToEmail)
                    : Result.Failure<SendForgotPasswordOtpViewModel>(ErrorMessages.EmailDidNotSendError);

        return (await SendOtpCodeAsync(user.ActiveCode, user.Mobile))
            .IsSuccess
                ? Result.Success(user.MapToSendForgotPasswordOtpViewModel(), SuccessMessages.OtpCodeSentSuccessfullyToMobile)
                : Result.Failure<SendForgotPasswordOtpViewModel>(ErrorMessages.SmsDidNotSendError);
    }
    
    #endregion

    #region VerifyForgotPasswordOtpAsync
    
    public async Task<Result<SendForgotPasswordOtpViewModel>> VerifyForgotPasswordOtpAsync(OtpCodeViewModel model)
    {
        string input = model.MobileOrEmail?.Trim() ?? "";
    
        bool isEmail = UserRegex.EmailRegex().IsMatch(input);
    
        var user = await userRepository.FirstOrDefaultAsync(x =>
            (isEmail ? x.Email == input : x.Mobile == input) && !x.IsDeleted);
    
        if (user is null)
            return Result.Failure<SendForgotPasswordOtpViewModel>(ErrorMessages.UserNotFoundError);
    
        if (user.ActiveCodeExpireTime < DateTime.Now || user.ActiveCode != model.Code)
            return Result.Failure<SendForgotPasswordOtpViewModel>(ErrorMessages.NotValidOtpCode);
    
        return Result.Success(new SendForgotPasswordOtpViewModel
        {
            UserId = user.Id,
            ActiveCodeExpireDateTime = user.ActiveCodeExpireTime!.Value.ToJavaScriptDateTimeStandard()
        });
    }
    
    #endregion
    
    #region ChangePasswordByResetAsync
    
    public async Task<Result> ChangePasswordByResetAsync(ChangePasswordViewModel model)
    {
        var user = await userRepository.GetByIdAsync(model.UserId);
        
        if (user == null || user.IsDeleted)
            return Result.Failure(ErrorMessages.UserNotFoundError);
    
        // if (user.ActiveCodeExpireTime < DateTime.Now || string.IsNullOrEmpty(user.ActiveCode))
        //     return Result.Failure(ErrorMessages.NotValidOtpCode);
    
        user.Password = model.Password.EncodePasswordSHA512();
        
        user.ActiveCode = null;
        user.ActiveCodeExpireTime = null;
    
        userRepository.Update(user);
        await userRepository.SaveChangesAsync();
    
        return Result.Success(SuccessMessages.PasswordChangedSuccessfully);
    }
    
    #endregion

    #endregion

    #region Client

    #region GetByIdForUserPanelAsync

    public async Task<Result<ClientUserViewModel>> GetByIdForUserPanelAsync(int id)
    {
        if (id < 1)
            return Result.Failure<ClientUserViewModel>(ErrorMessages.BadRequestError);

        var user = await userRepository.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (user is null)
            return Result.Failure<ClientUserViewModel>(ErrorMessages.NotFoundError);

        return user.MapToClientUserViewModel();
    }

    #endregion

    #region FillModelForUserPanelUpdateAsync

    public async Task<Result<ClientUpdateUserViewModel>> FillModelForUserPanelUpdateAsync(int id)
    {
        if (id < 1)
            return Result.Failure<ClientUpdateUserViewModel>(ErrorMessages.BadRequestError);

        string[] includes =
        [
            nameof(User.UserInformation)
        ];

        var user = await userRepository.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, includes: includes);

        if (user is null)
            return Result.Failure<ClientUpdateUserViewModel>(ErrorMessages.NotFoundError);

        return user.MapToClientUpdateUserViewModel();
    }

    #endregion

    #region UpdateAsync

    public async Task<Result> UpdateAsync(ClientUpdateUserViewModel model)
    {
        #region Validations

        if (model.Id < 1)
            return Result.Failure(ErrorMessages.BadRequestError);


        if (!model.NationalCode.IsNullOrEmptyOrWhiteSpace() && await userRepository.AnyAsync(x =>
                x.UserInformation.NationalCode == model.NationalCode && x.Id != model.Id && !x.IsDeleted &&
                !model.NationalCode.IsNullOrEmptyOrWhiteSpace()))
            return Result.Failure(string.Format(ErrorMessages.AlreadyExistError, "کد ملی"));

        #endregion

        string[] includes =
        [
            nameof(User.UserInformation)
        ];

        var user = await userRepository.FirstOrDefaultAsync(x => x.Id == model.Id && !x.IsDeleted, includes: includes);

        if (user is null)
            return Result.Failure(ErrorMessages.NotFoundError);

        user.MapToUser(model);

        userRepository.Update(user);
        await userRepository.SaveChangesAsync();

        return Result.Success(SuccessMessages.UpdateSuccessfullyDone);
    }

    #endregion

    #region UpdateAvatar

    public async Task<Result<string>> UpdateAvatar(ClientUpdateUserAvatarViewModel model)
    {
        if (model.UserId < 1)
            return Result.Failure<string>(ErrorMessages.BadRequestError);

        var user = await userRepository.FirstOrDefaultAsync(x => x.Id == model.UserId && !x.IsDeleted);

        if (user is null)
            return Result.Failure<string>(ErrorMessages.NotFoundError);

        #region Add User Avatar

        var result = await model.Avatar.AddImageToServer(FilePaths.UserImagePath, 300, 300,
            FilePaths.UserImageThumbPath, deleteFileName: user.AvatarImageName);

        if (result.IsFailure)
            return Result.Failure<string>(result.Message!);

        user.AvatarImageName = result.Value;

        #endregion

        userRepository.Update(user);
        await userRepository.SaveChangesAsync();

        return Result.Success(FilePaths.UserImagePath + result.Value, SuccessMessages.UpdateAvatarSuccessfullyDone);
    }

    #endregion

    #region DeleteAvatar

    public async Task<Result<string>> DeleteAvatar(int id)
    {
        if (id < 1)
            return Result.Failure<string>(ErrorMessages.BadRequestError);

        var user = await userRepository.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (user is null)
            return Result.Failure<string>(ErrorMessages.NotFoundError);

        user.AvatarImageName?.DeleteImage(FilePaths.UserImagePath, FilePaths.UserImageThumbPath);
        user.AvatarImageName = SiteTools.DefaultImageName;

        userRepository.Update(user);
        await userRepository.SaveChangesAsync();

        return Result.Success(FilePaths.UserImagePath + SiteTools.DefaultImageName,
            SuccessMessages.DeleteAvatarSuccessfullyDone);
    }

    #endregion

    #region ChangeUserMobile

    public async Task<Result> ChangeUserMobile(ClientUpdateMobileViewModel model, int userId)
    {
        if (model.Mobile.IsNullOrEmptyOrWhiteSpace())
            return Result.Failure(string.Format(ErrorMessages.RequiredError, "شماره موبایل"));

        if (model.ConfirmationCode.IsNullOrEmptyOrWhiteSpace())
            return Result.Failure(string.Format(ErrorMessages.RequiredError, "کد تایید"));

        if (!model.Mobile.StartsWith("0"))
            model.Mobile = string.Concat("0", model.Mobile);

        model.Mobile = model.Mobile.SanitizeTextAndTrim();

        if (await userRepository.AnyAsync(c => c.Mobile == model.Mobile && c.Id != userId))
            return Result.Failure(string.Format(ErrorMessages.DuplicatedError, "شماره موبایل"));

        var user = await userRepository.FirstOrDefaultAsync(c => c.Id == userId);

        if (user is null)
            return Result.Failure(ErrorMessages.NotFoundError);

        var result = await ConfirmOtpCodeAsync(user.Id, model.ConfirmationCode,OtpType.Mobile);

        if (result.IsFailure)
            return Result.Failure(result.Message);

        user.Mobile = model.Mobile;

        user.MobileActiveCode = null;
        user.MobileActiveCodeExpireTime = null;

        userRepository.Update(user);
        await userRepository.SaveChangesAsync();

        return Result.Success(SuccessMessages.SuccessfullyDone);
    }

    #endregion

    #region ChangeUserEmail

    public async Task<Result> ChangeUserEmail(ClientUpdateEmailViewModel model, int userId)
    {
        if (model.Email.IsNullOrEmptyOrWhiteSpace())
            return Result.Failure(string.Format(ErrorMessages.RequiredError, "ایمیل"));
    
        if (model.ConfirmationCode.IsNullOrEmptyOrWhiteSpace())
            return Result.Failure(string.Format(ErrorMessages.RequiredError, "کد تایید"));
    
    
        model.Email = model.Email.SanitizeTextAndTrim();
    
        if (await userRepository.AnyAsync(c => c.Email == model.Email && c.Id != userId))
            return Result.Failure(string.Format(ErrorMessages.DuplicatedError, "ایمیل"));
    
        var user = await userRepository.FirstOrDefaultAsync(c => c.Id == userId);
    
        if (user is null)
            return Result.Failure(ErrorMessages.NotFoundError);
    
        var result = await ConfirmOtpCodeAsync(user.Id, model.ConfirmationCode,OtpType.Email);
    
        if (result.IsFailure)
            return Result.Failure(result.Message);
    
        user.Email = model.Email;
    
        user.EmailActiveCode = null;
        user.EmailActiveCodeExpireTime = null;
    
        userRepository.Update(user);
        await userRepository.SaveChangesAsync();
    
        return Result.Success(SuccessMessages.SuccessfullyDone);
    }

    #endregion

    #region ChangeUserPassword

    public async Task<Result> AddUserPassword(ClientAddPasswordViewModel model, int userId)
    {
        if (model.OtpCode.IsNullOrEmptyOrWhiteSpace())
            return Result.Failure(string.Format(ErrorMessages.RequiredError, "کد تایید"));
    
        if (model.Password.IsNullOrEmptyOrWhiteSpace())
            return Result.Failure(string.Format(ErrorMessages.RequiredError, "رمز عبور")); 
        
        if (model.ConfirmPassword.IsNullOrEmptyOrWhiteSpace())
            return Result.Failure(string.Format(ErrorMessages.RequiredError, "تکرار رمز عبور"));
    
    
    
        var user = await userRepository.FirstOrDefaultAsync(c => c.Id == userId);
    
        if (user is null)
            return Result.Failure(ErrorMessages.UserNotFoundError);
    
        
        var result = await ConfirmOtpCodeAsync(user.Id, model.OtpCode,OtpType.Default);
    
        if (result.IsFailure)
            return Result.Failure(result.Message);
    
        user.Password = model.Password.EncodePasswordSHA512();
    
        user.ActiveCode = null;
        user.ActiveCodeExpireTime = null;
    
        userRepository.Update(user);
        await userRepository.SaveChangesAsync();
    
        return Result.Success(SuccessMessages.SuccessfullyDone);
    }
    
    public async Task<Result> ChangeUserPassword(ClientUpdatePasswordViewModel model, int userId)
    {
        if (model.CurrentPassword.IsNullOrEmptyOrWhiteSpace())
            return Result.Failure(string.Format(ErrorMessages.RequiredError, "کد تایید"));
    
        if (model.NewPassword.IsNullOrEmptyOrWhiteSpace())
            return Result.Failure(string.Format(ErrorMessages.RequiredError, "رمز عبور جدید")); 
        
        if (model.ConfirmNewPassword.IsNullOrEmptyOrWhiteSpace())
            return Result.Failure(string.Format(ErrorMessages.RequiredError, "تکرار رمز عبور جدید"));

    
        var user = await userRepository.FirstOrDefaultAsync(c => c.Id == userId);
    
        if (user is null)
            return Result.Failure(ErrorMessages.NotFoundError);
        
                
        string hashedCurrentPassword = model.CurrentPassword.EncodePasswordSHA512();

        if (user.Password != hashedCurrentPassword)
            return Result.Failure<ConfirmLoginViewModel>(ErrorMessages.CurrentPasswordNotCorrect);
    
    
        user.Password = model.NewPassword.EncodePasswordSHA512();
    
        user.ActiveCode = null;
        user.ActiveCodeExpireTime = null;
    
        userRepository.Update(user);
        await userRepository.SaveChangesAsync();
    
        return Result.Success(SuccessMessages.SuccessfullyDone);
    }

    #endregion
    
    #region ConfirmOtpCodeAsync
    
    public async Task<Result> ConfirmOtpCodeAsync(int userId, string code, OtpType type)
    {
        if (userId < 1 || string.IsNullOrEmpty(code))
            return Result.Failure(ErrorMessages.BadRequestError);

        var user = await userRepository.FirstOrDefaultAsync(x =>
            x.Id == userId && !x.IsDeleted);

        if (user is null)
            return Result.Failure<AuthenticateUserViewModel>(ErrorMessages.UserNotFoundError);

        switch (type)
        {
            case OtpType.Email:
                if (user.EmailActiveCodeExpireTime < DateTime.Now)
                    return Result.Failure<AuthenticateUserViewModel>(ErrorMessages.ExpireConfirmCodeError);

                if (user.EmailActiveCode is null || user.EmailActiveCode != code)
                    return Result.Failure<AuthenticateUserViewModel>(ErrorMessages.InvalidConfirmationCode);
                break;

            case OtpType.Mobile:
                if (user.MobileActiveCodeExpireTime < DateTime.Now)
                    return Result.Failure<AuthenticateUserViewModel>(ErrorMessages.ExpireConfirmCodeError);

                if (user.MobileActiveCode is null || user.MobileActiveCode != code)
                    return Result.Failure<AuthenticateUserViewModel>(ErrorMessages.InvalidConfirmationCode);
                break;
            
            case OtpType.Default:
                if (user.ActiveCodeExpireTime < DateTime.Now)
                    return Result.Failure<AuthenticateUserViewModel>(ErrorMessages.ExpireConfirmCodeError);

                if (user.ActiveCode is null || user.ActiveCode != code)
                    return Result.Failure<AuthenticateUserViewModel>(ErrorMessages.InvalidConfirmationCode);
                break;

            default:
                return Result.Failure<AuthenticateUserViewModel>(ErrorMessages.InvalidConfirmationCode);
        }

        userRepository.Update(user);
        await userRepository.SaveChangesAsync();

        return Result.Success();
    }

    #endregion

    #region GenerateAndSendOtpCodeForUserPanelAsync
    
    public async Task<Result> GenerateAndSendOtpCodeForUserPanelAsync(int userId, OtpType type,string? mobileOrEmail = null)
    {
        var user = await userRepository.FirstOrDefaultAsync(x =>
            x.Id == userId && !x.IsDeleted);

        if (user is null)
            return Result.Failure(ErrorMessages.UserNotFoundError);
        
        SetUserActiveCode(user, type);

        userRepository.Update(user);
        await userRepository.SaveChangesAsync();

        string? receiver;
        
        if (!mobileOrEmail.IsNullOrEmptyOrWhiteSpace())
        {
            receiver = mobileOrEmail;
        }
        else
        {
            receiver = type switch
            {
                OtpType.Email when !string.IsNullOrWhiteSpace(user.Email) => user.Email,
                OtpType.Mobile when !string.IsNullOrWhiteSpace(user.Mobile) => user.Mobile,
                _ => null
            };
        }
        
        if (receiver is null)
            return Result.Failure("آدرس ایمیل یا شماره موبایل کاربر یافت نشد.");

        return await SendOtpCodeAsync(user.ActiveCode, receiver);
    }


    #endregion

    #region GetConfirmationCodeExpireTimeAsync

    public async Task<Result<string>> GetConfirmationCodeExpireTimeAsync(int userId, OtpType type)
    {
        var user = await userRepository.FirstOrDefaultAsync(x => x.Id == userId && !x.IsDeleted);


        if (user == null)
            return Result.Failure<string>(ErrorMessages.UserNotFoundError);

        DateTime? expireTime = type switch
        {
            OtpType.Email => user.EmailActiveCodeExpireTime,
            OtpType.Mobile => user.MobileActiveCodeExpireTime,
            OtpType.Default => user.ActiveCodeExpireTime,
        };

        if (!expireTime.HasValue)
            return Result.Failure<string>("زمان انقضای کد فعال‌سازی یافت نشد.");

        return expireTime.Value.ToJavaScriptDateTimeStandard();
    }

    #endregion

    #endregion

    #region Helper

    private void SetUserActiveCode(User user, OtpType type)
    {
        string newCode;
        do
        {
            newCode = Common.GenerateRandomNumericCode(6);
        } while (type switch
                 {
                     OtpType.Email => user.EmailActiveCode == newCode,
                     OtpType.Mobile => user.MobileActiveCode == newCode,
                     OtpType.Default => user.ActiveCode == newCode
                 });

        switch (type)
        {
            case OtpType.Email:
                user.EmailActiveCode = newCode;
                user.EmailActiveCodeExpireTime = DateTime.Now.AddMinutes(3);
                break;

            case OtpType.Mobile:
                user.MobileActiveCode = newCode;
                user.MobileActiveCodeExpireTime = DateTime.Now.AddMinutes(1.5);
                break;

            default:
                user.ActiveCode = newCode;
                user.ActiveCodeExpireTime = DateTime.Now.AddMinutes(2);
                break;
        }
    }

    #endregion
}