using System.Security.Claims;
using Academy.Application.Statics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;

namespace Academy.Web.Extensions;

public static class AuthenticationExtension
{
    public static IServiceCollection AddApplicationAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            option.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }).AddCookie(options =>
        {
            options.LoginPath = "/login";
            options.LogoutPath = "/logout";
            options.ExpireTimeSpan = TimeSpan.FromDays(30);
            options.AccessDeniedPath = "/account/access-denied";
            options.Events.OnRedirectToLogin = context =>
            {
                if (context.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                {
                    string? returnUrl = context.Request.Path.ToString();

                    if (returnUrl.IsNullOrEmpty())
                        context.Response.Redirect("/login");
                    else
                        context.Response.Redirect($"/login?returnUrl={returnUrl}");
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                }

                return Task.CompletedTask;
            };
        }).AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = GoogleAuth.ClientId;
            googleOptions.ClientSecret = GoogleAuth.ClientSecret;
            googleOptions.SaveTokens = true;

            googleOptions.CallbackPath = "/signin-google";
            googleOptions.Scope.Add("profile");
            googleOptions.Scope.Add("email");
            googleOptions.AccessDeniedPath = "/account/access-denied";
            googleOptions.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
            googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
        });
        services.AddAuthorization();

        return services;
    }
}