using Academy.Web.Areas.UserPanel.Controllers.Common;
using Academy.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Areas.UserPanel.Controllers;

public class HomeController : UserPanelBaseController
{
    [HttpGet(RoutingExtension.UserPanel.Home.Index)]
    public IActionResult Index()
    {
        ShowToasterSuccessMessage("به پنل کاربری خوش آمدید");
        return View();
    }
}