using Newtonsoft.Json;

namespace Academy.Domain.DTOs.ArCaptcha.Response;

public record ArCaptchaVerificationResponseDto
{
    [JsonProperty("success")] 
    public bool Success { get; set; }
}