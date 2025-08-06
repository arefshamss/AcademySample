using Academy.Application.Services.Implementation;
using Academy.Application.Services.Interfaces;

namespace Academy.Web.Extensions;

internal static class HttpClientFactories
{
    internal static IServiceCollection AddHttpClientFactories(this IServiceCollection services)
    {
        services.AddHttpClient<ICaptchaService , CaptchaService>(client =>
        {
            client.BaseAddress = new ("https://www.google.com/recaptcha/api/");
        });
     
        return services;
    }
}