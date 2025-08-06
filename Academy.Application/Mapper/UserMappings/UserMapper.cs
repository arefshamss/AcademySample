using Academy.Application.Statics;
using Academy.Application.Extensions;
using Academy.Domain.Enums.User;
using Academy.Domain.Extensions;
using Academy.Domain.Models.User;
using Academy.Domain.ViewModels.User.Account;
using Academy.Domain.ViewModels.User.Admin;
using Academy.Domain.ViewModels.User.Client;

namespace Academy.Application.Mapper.UserMappings;

public static class UserMapper
{
    #region Admin

    public static AdminUserViewModel MapToAdminUserViewModel(this User model) =>
        new()
        {
            Id = model.Id,
            AvatarImageName = FilePaths.UserImagePath + model.AvatarImageName,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Mobile = model.Mobile,
            CreatedDate = model.CreatedDate,
            IsDeleted = model.IsDeleted,
            IsBanned = model.IsBanned,
            IsMobileActive = model.IsMobileActive,
            IsEmailActive = model.IsEmailActive,
            IsBannedFromTicket = model.IsBannedFromTicket,
            IsBannedFromComment = model.IsBannedFromComment,
        };

    public static AdminUserDetailsViewModel MapToAdminUserDetailsViewModel(this User model) =>
        new()
        {
            Id = model.Id,
            AvatarImageName = FilePaths.UserImagePath + model.AvatarImageName,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Mobile = model.Mobile,
            CreatedDate = model.CreatedDate,
            IsDeleted = model.IsDeleted,
            IsBanned = model.IsBanned,
            IsMobileActive = model.IsMobileActive,
            IsEmailActive = model.IsEmailActive,
            IsBannedFromTicket = model.IsBannedFromTicket,
            IsBannedFromComment = model.IsBannedFromComment,
            BirthDate = model.UserInformation?.BirthDate,
            PostalCode = model.UserInformation?.PostalCode,
            FatherName = model.UserInformation?.FatherName,
            ReferralSource = model.UserInformation?.ReferralSource ?? UserReferralSource.None,
            Gender = model.UserInformation?.Gender ?? UserGender.None,
            Address = model.UserInformation?.Address,
            NationalCode = model.UserInformation?.NationalCode,
            BirthCertificateNumber = model.UserInformation?.BirthCertificateNumber,
        };

    public static User MapToUser(this AdminCreateUserViewModel viewModel)
    {
        var user = new User
        {
            Mobile = viewModel.Mobile,
            Email = viewModel.Email,
            FirstName = viewModel.FirstName,
            LastName = viewModel.LastName,
            Password = viewModel.Password.EncodePasswordSHA512(),
            IsBanned = viewModel.IsBanned,
            IsBannedFromTicket = viewModel.IsBannedFromTicket,
            IsBannedFromComment = viewModel.IsBannedFromComment,
            IsMobileActive = viewModel.IsMobileActive,
            IsEmailActive = viewModel.IsEmailActive
        };
        user.UserInformation = new UserInformation
        {
            BirthDate = viewModel.BirthDate?.ToMiladiDateTime(),
            PostalCode = viewModel.PostalCode,
            FatherName = viewModel.FatherName,
            ReferralSource = viewModel.ReferralSource,
            Gender = viewModel.Gender,
            Address = viewModel.Address,
            NationalCode = viewModel.NationalCode,
            BirthCertificateNumber = viewModel.BirthCertificateNumber,
            User = user
        };

        return user;
    }


    public static void UpdateUser(this User model, AdminUpdateUserViewModel viewModel)
    {
        model.Id = viewModel.Id;
        model.FirstName = viewModel.FirstName;
        model.LastName = viewModel.LastName;
        model.Mobile = viewModel.Mobile;
        model.Email = viewModel.Email;
        if (viewModel.Password != null) model.Password = viewModel.Password.EncodePasswordSHA512();
        model.IsBanned = viewModel.IsBanned;
        model.IsBannedFromTicket = viewModel.IsBannedFromTicket;
        model.IsBannedFromComment = viewModel.IsBannedFromComment;
        model.IsMobileActive = viewModel.IsMobileActive;
        model.IsEmailActive = viewModel.IsEmailActive;

        if (model.UserInformation == null)
            model.UserInformation = new UserInformation { User = model };

        if (viewModel.BirthDate != null) model.UserInformation.BirthDate = viewModel.BirthDate.ToMiladiDateTime();
        model.UserInformation.PostalCode = viewModel.PostalCode;
        model.UserInformation.FatherName = viewModel.FatherName;
        model.UserInformation.ReferralSource = viewModel.ReferralSource;
        model.UserInformation.Gender = viewModel.Gender;
        model.UserInformation.Address = viewModel.Address;
        model.UserInformation.NationalCode = viewModel.NationalCode;
        model.UserInformation.BirthCertificateNumber = viewModel.BirthCertificateNumber;
    }

    public static AdminUpdateUserViewModel MapToAdminUpdateUserViewModel(this User model) =>
        new()
        {
            Id = model.Id,
            AvatarImageName = FilePaths.UserImagePath + model.AvatarImageName,
            Mobile = model.Mobile,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            IsBanned = model.IsBanned,
            IsBannedFromTicket = model.IsBannedFromTicket,
            IsBannedFromComment = model.IsBannedFromComment,
            IsMobileActive = model.IsMobileActive,
            IsEmailActive = model.IsEmailActive,
            BirthDate = model.UserInformation?.BirthDate.ToShamsi(),
            PostalCode = model.UserInformation?.PostalCode,
            FatherName = model.UserInformation?.FatherName,
            ReferralSource = model.UserInformation?.ReferralSource ?? UserReferralSource.None,
            Gender = model.UserInformation?.Gender ?? UserGender.None,
            Address = model.UserInformation?.Address,
            NationalCode = model.UserInformation?.NationalCode,
            BirthCertificateNumber = model.UserInformation?.BirthCertificateNumber
        };

    public static AdminUserExportDataViewModel MapToAdminUserExportDataViewModel(this AdminUserViewModel viewModel) =>
        new()
        {
            Id = viewModel.Id,
            Email = viewModel.Email,
            Mobile = viewModel.Mobile,
            FirstName = viewModel.FirstName,
            LastName = viewModel.LastName,
            CreatedDate = viewModel.CreatedDate.ToShamsi(),
            IsDeleted = viewModel.IsDeleted ? "حذف شده" : "حذف نشده",
            IsMobileActive = viewModel.IsMobileActive ? "فعال" : "غیرفعال",
            IsEmailActive = viewModel.IsEmailActive ? "فعال" : "غیرفعال",
            IsBanned = viewModel.IsBanned ? "مسدود شده" : "فعال",
            IsBannedFromComment = viewModel.IsBannedFromComment ? "مسدود شده" : "فعال",
            IsBannedFromTicket = viewModel.IsBannedFromTicket ? "مسدود شده" : "فعال",
        };

    public static AdminUserExportMobileDataViewModel MapToAdminUserExportMobileDataViewModel(
        this AdminUserViewModel viewModel) =>
        new()
        {
            Mobile = viewModel.Mobile,
        };

    #endregion

    #region Account

    public static User MapToUser(this LoginOrRegisterViewModel viewModel, bool isEmail) =>
        new()
        {
            Email = isEmail ? viewModel.MobileOrEmail : null,
            Mobile = isEmail ? null : viewModel.MobileOrEmail,
            UserInformation = new UserInformation()
        };
    
    public static ConfirmLoginViewModel MapToConfirmLoginViewModel(this User model, bool isLoginByPassword, bool isEmail) =>
        new()
        {
            MobileOrEmail = isEmail ? model.Email : model.Mobile,
            FullName = model.GetUserDisplayName(),
            UserId = model.Id,
            ActiveCode = model.ActiveCode,
            IsLoginByPassword = isLoginByPassword
        };
    
    public static AuthenticateUserViewModel MapToAuthenticateUserViewModel(this User model) =>
        new()
        {
            UserId = model.Id,
            MobileOrEmail = model.Email ?? model.Mobile,
            FullName = model.GetUserDisplayName(),
        };
    
    public static ResendOtpCodeViewModel MapToResendOtpCodeViewModel(this User model) =>
        new()
        {
            ActiveCodeExpireDateTime = model.ActiveCodeExpireTime!.Value.ToJavaScriptDateTimeStandard(),
        };    
    
    public static SendForgotPasswordOtpViewModel MapToSendForgotPasswordOtpViewModel (this User model) =>
        new()
        {
            UserId = model.Id,
            ActiveCodeExpireDateTime = model.ActiveCodeExpireTime!.Value.ToJavaScriptDateTimeStandard(),
        };

    #endregion

    #region Client

    public static ClientUserViewModel MapToClientUserViewModel(this User model) =>
        new()
        {
            Id = model.Id,
            AvatarImageName = FilePaths.UserImagePath + model.AvatarImageName,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Mobile = model.Mobile,
            CreatedDate = model.CreatedDate,
            IsMobileActive = model.IsMobileActive,
            IsEmailActive = model.IsEmailActive
        };

    public static ClientUpdateUserViewModel MapToClientUpdateUserViewModel(this User model) =>
        new()
        {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Mobile = model.Mobile,
            Email = model.Email,
            ActiveCode = model.ActiveCode,
            ActiveCodeExpireTime = model.ActiveCodeExpireTime,
            EmailActiveCode = model.EmailActiveCode,
            EmailActiveCodeExpireTime = model.EmailActiveCodeExpireTime,
            MobileActiveCode = model.MobileActiveCode,
            MobileActiveCodeExpireTime = model.MobileActiveCodeExpireTime,
            BirthDate = model.UserInformation?.BirthDate.ToShamsi(),
            PostalCode = model.UserInformation?.PostalCode,
            FatherName = model.UserInformation?.FatherName,
            ReferralSource = model.UserInformation?.ReferralSource ?? UserReferralSource.None,
            Gender = model.UserInformation?.Gender ?? UserGender.None,
            Address = model.UserInformation?.Address,
            NationalCode = model.UserInformation?.NationalCode,
            BirthCertificateNumber = model.UserInformation?.BirthCertificateNumber,
            HasPassword = !model.Password.IsNullOrEmptyOrWhiteSpace()
        };

    public static void MapToUser(this User model, ClientUpdateUserViewModel viewModel)
    {
        model.FirstName = viewModel.FirstName;
        model.LastName = viewModel.LastName;
        model.Mobile = viewModel.Mobile;
        model.Email = viewModel.Email;
        if (model.UserInformation == null)
            model.UserInformation = new UserInformation { User = model };

        if (viewModel.BirthDate != null) model.UserInformation.BirthDate = viewModel.BirthDate.ToMiladiDateTime();
        model.UserInformation.PostalCode = viewModel.PostalCode;
        model.UserInformation.FatherName = viewModel.FatherName;
        model.UserInformation.ReferralSource = viewModel.ReferralSource;
        model.UserInformation.Gender = viewModel.Gender;
        model.UserInformation.Address = viewModel.Address;
        model.UserInformation.NationalCode = viewModel.NationalCode;
        model.UserInformation.BirthCertificateNumber = viewModel.BirthCertificateNumber;
    }
    #endregion
}