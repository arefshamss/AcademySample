using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Academy.Web.Helpers;

public static class MvcViewHelper
{
    /// <summary>
    /// Returns the MVC view path (e.g. "~/Views/Home/Index.cshtml" or
    /// </summary>
    public static string GetDefaultViewPath(this ActionContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        
        if (context.ActionDescriptor is ControllerActionDescriptor cad)
        {
            var controller = cad.ControllerName;
            var action     = cad.ActionName;
            var area      = cad.RouteValues.TryGetValue("area", out var a) ? a : null;
            
            if (!string.IsNullOrEmpty(area))
            {
                return $"~/Areas/{area}/Views/{controller}/{action}.cshtml";
            }
            else
            {
                return $"~/Views/{controller}/{action}.cshtml";
            }
        }
        
        var rv = context.RouteData.Values;
        var ctrl   = rv["controller"]?.ToString() ?? "<unknown>";
        var act    = rv["action"]?.ToString()     ?? "<unknown>";
        var areaName = rv.ContainsKey("area") ? rv["area"]?.ToString() : null;
        
        return !string.IsNullOrEmpty(areaName)
            ? $"~/Areas/{areaName}/Views/{ctrl}/{act}.cshtml"
            : $"~/Views/{ctrl}/{act}.cshtml";
    }

}