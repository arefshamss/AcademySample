using Academy.Domain.Enums.SmsSetting;
using Academy.Domain.Shared;

namespace Academy.Application.Services.Interfaces;

public interface ISmsSenderService
{
    Task<Result> SendSmsAsync(string[] mobiles, string textMessage,SmsSettingSection section);
    
    Task<Result> SendSmsAsync(string mobile, string textMessage,SmsSettingSection section);
}