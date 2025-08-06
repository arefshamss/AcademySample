using Newtonsoft.Json;

namespace Academy.Domain.ViewModels.Captcha;

public class CaptchaVerificationResponseViewModel
{
    [JsonProperty("success")]
    public bool Success { get; set; }
}