using Academy.Web.Areas.Admin.Controllers.Common;
using Academy.Web.Attributes;
using Academy.Application.Statics;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Areas.Admin.Controllers;


public class HomeController : AdminBaseController
{
    [HttpGet , UserActivityLog(Description = ActivityMessages.AdminIndexPage)]
    public IActionResult Index() => View();
}