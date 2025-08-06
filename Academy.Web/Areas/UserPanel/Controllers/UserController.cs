using Academy.Web.Areas.UserPanel.Controllers.Common;
using Academy.Web.Extensions;
using Academy.Application.Extensions;
using Academy.Application.Services.Interfaces;
using Academy.Application.Tools;
using Academy.Domain.Enums.User;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.User.Account;
using Academy.Domain.ViewModels.User.Client;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Areas.UserPanel.Controllers;

public class UserController(IUserService userService) : UserPanelBaseController
{
    #region Update

    [HttpGet(RoutingExtension.UserPanel.User.Update)]
    public async Task<IActionResult> Update()
    {
        var result = await userService.FillModelForUserPanelUpdateAsync(User.GetUserId());

        if (result.IsFailure)
        {
            ShowToasterErrorMessage(result.Message);
            return RedirectToAction("Index", "Home", new { area = "UserPanel" });
        }

        return View(result.Value);
    }

    [HttpPost(RoutingExtension.UserPanel.User.Update)]
    public async Task<IActionResult> Update(ClientUpdateUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ShowToasterErrorMessage(ModelState.GetModelErrorsAsString());
            return View(model);
        }

        var result = await userService.UpdateAsync(model);

        if (result.IsFailure)
        {
            ShowToasterErrorMessage(result.Message);
            return View(model);
        }

        ShowToasterSuccessMessage(result.Message);
        return RedirectToAction("Update", "User", new { area = "UserPanel" });
    }

    #endregion

    #region UpdateAvatar

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateAvatar(ClientUpdateUserAvatarViewModel model)
    {
        model.UserId = User.GetUserId();

        if (!ModelState.IsValid)
            return BadRequest(ModelState.GetModelErrorsAsString());

        var result = await userService.UpdateAvatar(model);

        return result.IsSuccess
            ? Ok(new JsonResult(new
            {
                Message = result.Message,
                AvatarName = result.Value
            }))
            : BadRequest(result.Message);
    }

    [HttpGet]
    public async Task<IActionResult> DeleteAvatar()
    {
        var result = await userService.DeleteAvatar(User.GetUserId());
        return result.IsSuccess
            ? Ok(new JsonResult(new
            {
                Message = result.Message,
                AvatarName = result.Value
            }))
            : BadRequest(result.Message);
    }

    #endregion

    #region Edit Mobile

    [HttpGet]
    public async Task<IActionResult> EditMobileModal()
    {
        var userId = User.GetUserId();
        var user = await userService.FillModelForUserPanelUpdateAsync(userId);
        
        if (user.IsFailure)
            return BadRequest(user.Message);

        if (user.Value.MobileActiveCode is null || user.Value.MobileActiveCodeExpireTime < DateTime.Now)
        {
            var result = await userService.GenerateAndSendOtpCodeForUserPanelAsync(user.Value.Id,OtpType.Mobile);
            
            if (result.IsFailure)
                return BadRequest(result.Message);
        }
        
        await TrySetActiveCodeExpireTimeAsync(userId, OtpType.Mobile);
        
        ViewData["CurrentMobile"] = user.Value.Mobile;
        
        return PartialView("_EditMobile");
    }


    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditMobileModal(ClientUpdateMobileViewModel model)
    {
        var userId = User.GetUserId();
        
        if (!ModelState.IsValid)
        {
            await TrySetActiveCodeExpireTimeAsync(userId, OtpType.Mobile);
            return BadRequest(ModelState.GetModelErrorsAsString());
        }

        var result = await userService.ChangeUserMobile(model, User.GetUserId());

        if (result.IsFailure)
        {
            await TrySetActiveCodeExpireTimeAsync(userId, OtpType.Mobile);
            return BadRequest(result.Message);
        }

        ShowToasterSuccessMessage(SuccessMessages.SuccessfullyDone);
        return Ok();
    }

    #endregion
    
    #region Edit Email

    [HttpGet]
    public async Task<IActionResult> EditEmailModal()
    {
        var userId = User.GetUserId();
        var user = await userService.FillModelForUserPanelUpdateAsync(userId);

        if (user.IsFailure)
            return BadRequest(user.Message);

        if (user.Value.EmailActiveCode is null || user.Value.EmailActiveCodeExpireTime < DateTime.Now)
        {
            var result = await userService.GenerateAndSendOtpCodeForUserPanelAsync(user.Value.Id,OtpType.Email);
            
            if (result.IsFailure)
                return BadRequest(result.Message);
        }
        
        await TrySetActiveCodeExpireTimeAsync(userId, OtpType.Email);
        
        ViewData["CurrentEmail"] = user.Value.Email;
        
        return PartialView("_EditEmail");
}


    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditEmailModal(ClientUpdateEmailViewModel model)
    {
        var userId = User.GetUserId();
        
        if (!ModelState.IsValid)
        {
        await TrySetActiveCodeExpireTimeAsync(userId, OtpType.Email);
            return BadRequest(ModelState.GetModelErrorsAsString());
        }
        
        var result = await userService.ChangeUserEmail(model, userId);

        if (result.IsFailure)
        {
        await TrySetActiveCodeExpireTimeAsync(userId, OtpType.Email);
            return BadRequest(result.Message);
        }

        ShowToasterSuccessMessage(SuccessMessages.SuccessfullyDone);
        return Ok();
    }

    #endregion

    #region Change Password
    
    [HttpGet]
    public async Task<IActionResult> ChangePasswordModal()
    {
        var userId = User.GetUserId();
        
        var user = await userService.FillModelForUserPanelUpdateAsync(userId);

        if (user.IsFailure)
            return BadRequest(user.Message);

        return PartialView("_ChangePassword");
    }


    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePasswordModal(ClientUpdatePasswordViewModel model)
    {
        var userId = User.GetUserId();
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.GetModelErrorsAsString());
        }
        
        var result = await userService.ChangeUserPassword(model, userId);

        if (result.IsFailure)
        {
            return BadRequest(result.Message);
        }

        ShowToasterSuccessMessage(SuccessMessages.SuccessfullyDone);
        return Ok();
    }

    #endregion
    
    #region Insert Mobile

    [HttpGet]
    public async Task<IActionResult> InsertMobileModal(string mobile)
    {
        bool isValidMobile = UserRegex.MobileRegex().IsMatch(mobile.Trim());

        if (!isValidMobile)
            return BadRequest(ErrorMessages.NotValidMobile);
        
        var userId = User.GetUserId();
        var user = await userService.FillModelForUserPanelUpdateAsync(userId);

        if (user.IsFailure)
            return BadRequest(user.Message);

        if (user.Value.MobileActiveCode is null || user.Value.MobileActiveCodeExpireTime < DateTime.Now)
        {
            var result = await userService.GenerateAndSendOtpCodeForUserPanelAsync(user.Value.Id,OtpType.Mobile,mobile);
            
            if (result.IsFailure)
                return BadRequest(result.Message);
        }
        
        await TrySetActiveCodeExpireTimeAsync(userId, OtpType.Mobile);
        
        ViewData["NewMobile"] = mobile;
        
        return PartialView("_InsertMobile");
    }
    
    #endregion
    
    #region Insert Email

    [HttpGet]
    public async Task<IActionResult> InsertEmailModal(string email)
    {
        bool isValidEmail = UserRegex.EmailRegex().IsMatch(email.Trim());

        if (!isValidEmail)
            return BadRequest(ErrorMessages.NotValidEmail);
        
        var userId = User.GetUserId();
        
        var user = await userService.FillModelForUserPanelUpdateAsync(userId);

        if (user.IsFailure)
            return BadRequest(user.Message);

        if (user.Value.EmailActiveCode is null || user.Value.EmailActiveCodeExpireTime < DateTime.Now)
        {
            var result = await userService.GenerateAndSendOtpCodeForUserPanelAsync(user.Value.Id,OtpType.Email,email.Trim());
            
            if (result.IsFailure)
                return BadRequest(result.Message);
        }
        await TrySetActiveCodeExpireTimeAsync(userId, OtpType.Email);
        
        ViewData["NewEmail"] = email;

        return PartialView("_InsertEmail");
}

    #endregion

    #region Add Password

    [HttpGet]
    public async Task<IActionResult> AddPasswordModal()
    {
        var userId = User.GetUserId();
        var user = await userService.FillModelForUserPanelUpdateAsync(userId);
        
        if (user.IsFailure)
            return BadRequest(user.Message);
        
        var mobileOrEmail = user.Value.Mobile ?? user.Value.Email;
        
        if(mobileOrEmail is null)
            return BadRequest(ErrorMessages.NotValidMobileOrEmail);


        if (user.Value.ActiveCode is null || user.Value.ActiveCodeExpireTime < DateTime.Now)
        {
            var result = await userService.GenerateAndSendOtpCodeForUserPanelAsync(user.Value.Id,OtpType.Default,mobileOrEmail);
            
            if (result.IsFailure)
                return BadRequest(result.Message);
        }
        
        await TrySetActiveCodeExpireTimeAsync(userId, OtpType.Default);

        ViewData["mobileOrEmail"] = mobileOrEmail;
        
        return PartialView("_AddPassword");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPasswordModal(ClientAddPasswordViewModel model)
    {
        var userId = User.GetUserId();
        
        if (!ModelState.IsValid)
        {
            await TrySetActiveCodeExpireTimeAsync(userId, OtpType.Default);
            return BadRequest(ModelState.GetModelErrorsAsString());
        }
        
        var result = await userService.AddUserPassword(model, userId);

        if (result.IsFailure)
        {
            await TrySetActiveCodeExpireTimeAsync(userId, OtpType.Default);
            return BadRequest(result.Message);
        }

        ShowToasterSuccessMessage(SuccessMessages.SuccessfullyDone);
        return Ok();
    }
    
    #endregion

    #region Helpers

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