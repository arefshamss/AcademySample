using Academy.Domain.Enums.EmailSmtp;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.EmailSmtp;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.Application.Services.Interfaces;

public interface IEmailSmtpService
{
    Task<FilterAdminSideEmailSmtpViewModel> FilterAsync(FilterAdminSideEmailSmtpViewModel filter);

    Task<Result> CreateAsync(AdminSideCreateEmailSmtpViewModel model);

    Task<Result<AdminSideUpdateEmailSmtpViewModel>> FillModelForEditAsync(short id);

    Task<Result> UpdateAsync(AdminSideUpdateEmailSmtpViewModel model);

    Task<Result> SoftDeleteOrRecoverAsync(short id);

    Task<Result<EmailSmtpViewModel>> GetDefaultSimpleEmailSmtpAsync();

    Task<Result<EmailSmtpViewModel>> GetDefaultMailServerEmailSmtpAsync();

    Task<Result<SelectList>> GetForSelectList(EmailSmtpType smtpType, short? selectedValue = null);
}