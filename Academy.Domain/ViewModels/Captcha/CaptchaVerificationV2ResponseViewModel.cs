using Newtonsoft.Json;

namespace Academy.Domain.ViewModels.Captcha;

public class CaptchaVerificationV2ResponseViewModel:CaptchaVerificationResponseViewModel
{
    [JsonProperty("challenge_ts")]
    public DateTime Date { get; set; }

    [JsonProperty("hostname")]
    public string Hostname { get; set; }

    public override string ToString() => $"{nameof(Success)}: {Success}, {nameof(Date)}: {Date}, {nameof(Hostname)}: {Hostname}";
}