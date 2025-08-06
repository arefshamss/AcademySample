using Academy.Application.Services.Interfaces;
using Academy.Domain.Contracts;
using Academy.Domain.Enums.SmsProvider;
using Academy.Domain.Enums.SmsSetting;
using Academy.Domain.Shared;
using Kavenegar;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PARSGREEN.RESTful.SMS;

namespace Academy.Application.Services.Implementation;

public class SmsSenderService(
    ILogger<SmsSenderService> logger,
    ISmsProviderRepository smsProviderRepository,
    ISmsSettingRepository smsSettingRepository)
    : ISmsSenderService
{
    public async Task<Result> SendSmsAsync(string[] mobiles, string textMessage, SmsSettingSection section)
    {
        var smsProviderType = (await smsSettingRepository.FirstOrDefaultAsync(s => s.SmsSettingSection == section))?
            .SmsProviderType;

        try
        {
            switch (smsProviderType)
            {
                case SmsProviderType.ParsGreen:
                    await ParsGreenSendSms(mobiles, textMessage);
                    break;
                
                case SmsProviderType.Kavenegar:
                    await KevenegarSendSms(mobiles, textMessage);
                    break;
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"sms sending failed.");
            return Result.Failure(ErrorMessages.SmsDidNotSendError);
        }
    }

    public async Task<Result> SendSmsAsync(string mobile, string textMessage, SmsSettingSection section)
        => await SendSmsAsync(new[] { mobile }, textMessage, section);

    #region Helpers

    private async Task ParsGreenSendSms(string[] mobiles, string textMessage)
    {
        var parsGreenProvider =
            await smsProviderRepository.FirstOrDefaultAsync(s =>
                s.SmsProviderType == SmsProviderType.ParsGreen);

        if (parsGreenProvider is null)
            throw new Exception("pars green provider not found!!!");


        var message = new Message(parsGreenProvider.ApiKey);


        var parsGreenResult = message.SendSms(
            $"{Environment.NewLine}{textMessage}", mobiles);

        if (parsGreenResult is not null && parsGreenResult.R_Success) return;
       
        string errorMessage =
            $"sms sending failed. ParsGreen response: {JsonConvert.SerializeObject(parsGreenResult)}";
        logger.LogWarning(errorMessage);
        
        throw new Exception(errorMessage);
    }

    private async Task KevenegarSendSms(string[] mobiles, string textMessage)
    {
        var kavenegarProvider =
            await smsProviderRepository.FirstOrDefaultAsync(s =>
                s.SmsProviderType == SmsProviderType.Kavenegar);

        if (kavenegarProvider is null)
            throw new Exception("kavenegar provider not found!!!");
        
        var api = new KavenegarApi( kavenegarProvider.ApiKey);
        var kavenegarResult= api.SendArray(new List<string>(), mobiles.ToList(), new List<string>(){textMessage});

        if (kavenegarResult is not null && kavenegarResult.Any(s=>s.Status==5)) return;
       
        string errorMessage =
            $"sms sending failed. Kavenegar response: {JsonConvert.SerializeObject(kavenegarResult)}";
        
        logger.LogWarning(errorMessage);
        
        throw new Exception(errorMessage);
    }

    #endregion
}