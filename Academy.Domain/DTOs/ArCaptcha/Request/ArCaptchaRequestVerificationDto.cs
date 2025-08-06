using Newtonsoft.Json;

namespace Academy.Domain.DTOs.ArCaptcha.Request;

public record ArCaptchaRequestVerificationDto
{
    [JsonProperty("challenge_id")]
    public required string ChallengeId { get; set; }

    [JsonProperty("site_key")]
    public required string SiteKey { get; set; }
    
    [JsonProperty("secret_key")]
    public required string SecretKey { get; set; }
}