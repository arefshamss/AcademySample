using Academy.Application.Extensions;
using Academy.Application.Services.Interfaces;
using Academy.Domain.ViewModels.Sidebar;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Areas.UserPanel.Components;

public class UserPanelSidebarViewComponent(ITicketService ticketService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = User.GetUserId();

        var result = await ticketService.GetUnreadTicketsCountAsync(userId);

        var vm = new UserPanelSidebarViewModel
        {
            NewTicketsCount = result
        };

        return View("UserPanelSidebar", vm);
    }
}
