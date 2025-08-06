using Newtonsoft.Json;

namespace Academy.Domain.ViewModels.Captcha;

public class CaptchaVerificationV3ResponseViewModel: CaptchaVerificationV2ResponseViewModel
{
    [JsonProperty("score")]
    public decimal Score { get; set; }

    [JsonProperty("action")]
    public string Action { get; set; }

    public override string ToString() => $"{base.ToString()}, {nameof(Score)}: {Score}, {nameof(Action)}: {Action}";
}