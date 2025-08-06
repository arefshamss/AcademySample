using System.Security.Claims;
using Academy.Web.Controllers.Common;
using Academy.Web.Extensions;
using Academy.Application.Services.Interfaces;
using Academy.Domain.Enums.User;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.User.Account;
using Academy.Domain.ViewModels.User.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Controllers;

public class AccountController(IUserService userService) : SiteBaseController
{
    #region GoogleSignIn

    [HttpGet(RoutingExtension.Site.Account.GoogleSignIn)]
    public IActionResult GoogleSignIn(string returnUrl = RoutingExtension.UserPanel.Home.Index)
    {
        var redirectUrl = Url.Action("GoogleResponse", new { returnUrl });
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    public async Task<IActionResult> GoogleResponse(string returnUrl = RoutingExtension.UserPanel.Home.Index)
    {
        var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        if (!result.Succeeded)
        {
            ShowToasterErrorMessage(ErrorMessages.GoogleAuthBadRequestError);
            return RedirectToAction("Logout", "Account");
        }

        var claims = result.Principal.Identities.FirstOrDefault()?.Claims.ToList();

        var confirmLoginResult = await userService.ConfirmLoginForGoogleAsync(new ConfirmLoginForGoogleViewModel()
        {
            Email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
            Mobile = claims?.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value,
            GoogleAuthenticationId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
            FirstName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
            LastName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value,
        });

        if (confirmLoginResult.IsSuccess)
        {
            await LoginUserAsync(confirmLoginResult.Value);
            return Redirect(returnUrl);
        }

        ShowToasterErrorMessage(confirmLoginResult.Message);
        return RedirectToAction("Logout", "Account");
    }

    #endregion

    #region LoginOrRegister

    [HttpGet(RoutingExtension.Site.Account.Login)]
    public IActionResult LoginOrRegister(string? mobileOrEmail = null, bool isLoginByPassword = false,
        string returnUrl = null)
    {
        if (UserIsAuthenticated())
            return RedirectToAction("Index", "Home");

        return View(new LoginOrRegisterViewModel
        {
            IsLoginByPassword = isLoginByPassword,
            ReturnUrl = returnUrl
        });
    }

    [HttpPost(RoutingExtension.Site.Account.Login)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginOrRegister(LoginOrRegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ShowToasterErrorMessage(ModelState.GetModelErrorsAsString());
            return View(model);
        }

        var result = await userService.ConfirmLoginOrRegisterAsync(model);

        if (result.IsFailure)
        {
            ShowToasterErrorMessage(result.Message);
            return View(model);
        }

        if (result.Value.IsLoginByPassword)
        {
            await LoginUserAsync(new()
            {
                FullName = result.Value.FullName,
                MobileOrEmail = result.Value.MobileOrEmail,
                UserId = result.Value.UserId
            });

            ShowToasterSuccessMessage(result.Message);
            if (Url.IsLocalUrl(model.ReturnUrl))
                return LocalRedirect(model.ReturnUrl);

            return RedirectToAction("Index", "Home", new { area = "UserPanel" });
        }

        var sendOtpResult = await userService.SendOtpCodeAsync(result.Value.ActiveCode!, result.Value.MobileOrEmail);

        if (sendOtpResult.IsFailure)
        {
            ShowToasterErrorMessage(sendOtpResult.Message);
            return View(model);
        }

        TempData["MobileOrEmail"] = model.MobileOrEmail;
        TempData["UserId"] = result.Value.UserId;
        ShowToasterSuccessMessage(SuccessMessages.OtpCodeSentSuccessfully);

        return RedirectToAction("ConfirmOtpCode", new { returnUrl = model.ReturnUrl });
    }

    #endregion

    #region Confirm Otp

    [HttpGet(RoutingExtension.Site.Account.ConfirmOtp)]
    public async Task<IActionResult> ConfirmOtpCode(string returnUrl = null)
    {
        if (UserIsAuthenticated())
            return RedirectToAction("Index", "Home", new { area = "UserPanel" });

        var userId = PeekTempData("UserId") as int? ?? 0;
        var mobileOrEmail = PeekTempData("MobileOrEmail") as string;

        await TrySetActiveCodeExpireTimeAsync(userId, OtpType.Default);


        return View(new OtpCodeViewModel
        {
            ReturnUrl = returnUrl,
            UserId = userId,
            MobileOrEmail = mobileOrEmail
        });
    }

    [HttpPost(RoutingExtension.Site.Account.ConfirmOtp)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmOtpCode(OtpCodeViewModel model)
    {
        if (UserIsAuthenticated())
            return RedirectToAction("Index", "Home", new { area = "UserPanel" });

        model.ReturnUrl ??= null;

        if (!ModelState.IsValid)
        {
            await TrySetActiveCodeExpireTimeAsync(model.UserId, OtpType.Default);
            ShowToasterErrorMessage(ModelState.GetModelErrorsAsString());
            return View(model);
        }

        var result = await userService.ConfirmOtpCodeAsync(model);

        if (result.IsFailure)
        {
            await TrySetActiveCodeExpireTimeAsync(model.UserId, OtpType.Default);
            ShowToasterErrorMessage(result.Message);

            return View(model);
        }
        
        RemoveTempData("MobileOrEmail");
        RemoveTempData("UserId");
        await LoginUserAsync(result.Value);
        ShowToasterSuccessMessage(SuccessMessages.LoginSuccessfullyDone);
        if (Url.IsLocalUrl(model.ReturnUrl))
            return LocalRedirect(model.ReturnUrl);

        return RedirectToAction("Index", "Home", new { area = "UserPanel" });

    }

    #endregion

    #region Resend Otp Code

    [HttpGet(RoutingExtension.Site.Account.Resend.Otp)]
    public async Task<IActionResult> ResendOtpCode(string mobileOrEmail)
    {
        var result = await userService.ResendOtpCodeAsync(mobileOrEmail);
        TempData.Keep("MobileOrEmail");
        return result.IsFailure
            ? BadRequest(result.Message)
            : Ok(new JsonResult(new
            {
                message = result.Message,
                otpExpire = result.Value.ActiveCodeExpireDateTime
            }));
    }

    #endregion

    #region Logout

    [HttpGet(RoutingExtension.Site.Account.Logout)]
    public async Task<IActionResult> Logout()
    {
        ShowToasterSuccessMessage(SuccessMessages.LogoutSuccessfullyDone);
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home", new { area = "" });
    }

    #endregion

    #region ForgotPassword

    [HttpGet(RoutingExtension.Site.Account.ForgotPassword)]
    public IActionResult ForgotPassword()
    {
        return View(new ForgotPasswordViewModel());
    }

    [HttpPost(RoutingExtension.Site.Account.ForgotPassword)]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await userService.SendForgotPasswordOtpAsync(model.MobileOrEmail);

        if (result.IsFailure)
        {
            ShowToasterErrorMessage(result.Message!);
            return View(model);
        }

        TempData["ResetUserId"] = result.Value.UserId;
        await TrySetActiveCodeExpireTimeAsync(model.MobileOrEmail);

        return RedirectToAction("VerifyForgotPasswordOtp", new { mobileOrEmail = model.MobileOrEmail });
    }

    #endregion

    #region VerifyResetPassword

    [HttpGet(RoutingExtension.Site.Account.VerifyResetPassword)]
    public async Task<IActionResult> VerifyForgotPasswordOtpAsync(string mobileOrEmail, string returnUrl = null)
    {
        if (UserIsAuthenticated())
            return RedirectToAction("Index", "Home", new { area = "UserPanel" });

        await TrySetActiveCodeExpireTimeAsync(mobileOrEmail);

        return View(new OtpCodeViewModel
        {
            UserId = TempData["ResetUserId"] is int id ? id : 0,
            ReturnUrl = returnUrl,
            MobileOrEmail = mobileOrEmail,
        });
    }

    [HttpPost(RoutingExtension.Site.Account.VerifyResetPassword)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> VerifyForgotPasswordOtpAsync(OtpCodeViewModel model)
    {
        if (UserIsAuthenticated())
            return RedirectToAction("Index", "Home", new { area = "UserPanel" });

        model.ReturnUrl ??= null;


        if (!ModelState.IsValid)
        {
            await TrySetActiveCodeExpireTimeAsync(model.MobileOrEmail);
            TempData["MobileOrEmail"] = model.MobileOrEmail;
            ShowToasterErrorMessage(ModelState.GetModelErrorsAsString());
            return View(model);
        }

        var result = await userService.VerifyForgotPasswordOtpAsync(model);

        if (result.IsFailure)
        {
            await TrySetActiveCodeExpireTimeAsync(model.MobileOrEmail);
            TempData["MobileOrEmail"] = model.MobileOrEmail;
            ShowToasterErrorMessage(result.Message);

            return View(model);
        }

        RemoveTempData("MobileOrEmail");
        RemoveTempData("UserId");
        return RedirectToAction("ChangePassword", new { userId = model.UserId });
    }

    #endregion

    #region ChangePassword

    [HttpGet(RoutingExtension.Site.Account.ChangePassword)]
    public IActionResult ChangePassword(int userId)
    {
        if (userId < 1)
            return RedirectToAction("LoginOrRegister");

        return View(new ChangePasswordViewModel { UserId = userId });
    }

    [HttpPost(RoutingExtension.Site.Account.ChangePassword)]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await userService.ChangePasswordByResetAsync(model);
        
        if (result.IsFailure)
        {
            ShowToasterErrorMessage(result.Message!);
            return View(model);
        }

        ShowToasterSuccessMessage(SuccessMessages.PasswordChangedSuccessfully);
        return RedirectToAction("LoginOrRegister");
    }

    #endregion

    #region Helpers

    private bool UserIsAuthenticated()
        => User.Identity?.IsAuthenticated ?? false;

    private async Task LoginUserAsync(AuthenticateUserViewModel model)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, model.UserId.ToString()),
            new(ClaimTypes.Name, model.FullName ?? ""),
            new("MobileOrEmail", model.MobileOrEmail),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);
        var properties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
        };
        await HttpContext.SignInAsync(principal, properties);
    }

    private async Task<bool> TrySetActiveCodeExpireTimeAsync(string mobileOrEmail)
    {
        var result = await userService.GetActiveCodeExpireTimeAsync(mobileOrEmail);
        if (result.IsFailure)
            return false;

        ViewData["ActiveCodeExpireTime"] = result.Value;
        return true;
    }

    private async Task<bool> TrySetActiveCodeExpireTimeAsync(int userId, OtpType type)
    {
        var result = await userService.GetConfirmationCodeExpireTimeAsync(userId, type);
        if (result.IsFailure)
            return false;

        ViewData["ActiveCodeExpireTime"] = result.Value;
        return true;
    }

    #endregion
}