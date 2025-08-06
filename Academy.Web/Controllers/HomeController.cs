using System.Diagnostics;
using Academy.Web.Controllers.Common;
using Academy.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Controllers;

public class HomeController : SiteBaseController
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ShowToasterSuccessMessage("خوش امدید");
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
