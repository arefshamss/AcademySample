using Academy.Web.Areas.MasterPanel.Controllers.Common;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Areas.MasterPanel.Controllers;

public class HomeController : MasterPanelBaseController
{
    public IActionResult Index() => View();
}