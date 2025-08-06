using System.Linq.Expressions;
using Academy.Domain.ViewModels.Common;
using Academy.Domain.ViewModels.EmailSmtp;

namespace Academy.Application.Mapper.EmailSmtp;

public static class EmailSmtpMapper
{
    public static AdminSideEmailSmtpViewModel ToAdminSideEmailSmtpViewModel(
        this Domain.Models.EmailSmtp.EmailSmtp model) =>
        new()
        {
            EmailAddress = model.EamilAddress,
            EmailSmtpType = model.EmailSmtpType,
            EnableSSL = model.EnableSSL,
            Id = model.Id,
            IsDeleted = model.IsDeleted,
            Port = model.Port,
            SmtpAddress = model.SmtpAddress,
            DisplayName = model.DisplayName,
        };

    public static Domain.Models.EmailSmtp.EmailSmtp ToEmailSmtp(this AdminSideCreateEmailSmtpViewModel model) =>
        new()
        {
            EamilAddress = model.EmailAddress,
            EmailSmtpType = model.EmailSmtpType,
            EnableSSL = model.EnableSSL,
            Password = model.Password,
            Port = model.Port,
            SmtpAddress = model.SmtpAddress,
            DisplayName = model.DisplayName,
        };

    public static AdminSideUpdateEmailSmtpViewModel ToUpdateAdminSideEmailSmtpViewModel(
        this Domain.Models.EmailSmtp.EmailSmtp model) =>
        new()
        {
            Id = model.Id,
            Port = model.Port,
            SmtpAddress = model.SmtpAddress,
            EmailAddress = model.EamilAddress,
            EmailSmtpType = model.EmailSmtpType,
            EnableSSL = model.EnableSSL,
            Password = model.Password,
            DisplayName = model.DisplayName
        };

    public static void ToEmailSmtp(this AdminSideUpdateEmailSmtpViewModel model,
        Domain.Models.EmailSmtp.EmailSmtp emailSmtp)
    {
        emailSmtp.EamilAddress = model.EmailAddress;
        emailSmtp.EmailSmtpType = emailSmtp.EmailSmtpType;
        emailSmtp.EnableSSL = model.EnableSSL;
        emailSmtp.Password = model.Password;
        emailSmtp.Port = model.Port;
        emailSmtp.SmtpAddress = model.SmtpAddress;
        emailSmtp.DisplayName = model.DisplayName;
        emailSmtp.EmailSmtpType = model.EmailSmtpType;
    }

    public static EmailSmtpViewModel ToEmailSmtpViewModel(this Domain.Models.EmailSmtp.EmailSmtp model)
        => new()
        {
            Id = model.Id,
            EmailAddress = model.EamilAddress,
            SmtpAddress = model.SmtpAddress,
            Port = model.Port,
            EnableSSL = model.EnableSSL,
            EmailSmtpType = model.EmailSmtpType,
            DisplayName = model.DisplayName,
            Password = model.Password
        };

    public static Expression<Func<Domain.Models.EmailSmtp.EmailSmtp, SelectViewModel<short>>> MapToSelectViewModel
        => emailSmtp => new SelectViewModel<short>()
        {
            Id = emailSmtp.Id,
            DisplayValue = $"{emailSmtp.DisplayName} - {emailSmtp.EamilAddress}",
        };
}