using Academy.Application.Extensions;
using Academy.Application.Services.Interfaces;
using Academy.Domain.ViewModels.User.Client;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Areas.UserPanel.Components;

public class UserInformationViewComponent(IUserService userService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = User.GetUserId();   
        
        var result = await userService.GetByIdForUserPanelAsync(userId);

        return View("UserInformation", result.Value);
    }
}
