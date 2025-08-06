using System.Reflection;
using Academy.Web.Attributes;
using Academy.Web.Exceptions;
using Academy.Application.Extensions;
using Academy.Application.Services.Interfaces;
using Academy.Domain.ViewModels.UserActivity;
using Academy.Web.Extensions;
using DeviceDetectorNET;
using DeviceDetectorNET.Cache;
using DeviceDetectorNET.Parser;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Bson;

namespace Academy.Web.ActionFilters;

public class UserActivityLogFilter(IUserActivityService userActivityService) : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if(!context.HttpContext.Request.Path.ToString().ToLower().Contains("/admin"))
            return;
        
        var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

        var userActivityLogAttribute = actionDescriptor?.MethodInfo.GetCustomAttribute<UserActivityLogAttribute>();

        var ignoreActivityLogAttribute = actionDescriptor?.MethodInfo.GetCustomAttribute<IgnoreUserActivityAttribute>();

        if (ignoreActivityLogAttribute is not null) return;

        if (userActivityLogAttribute == null)
        {
            string areaName = context.RouteData.Values["area"]?.ToString() ?? "NULL";
            string controller = context.RouteData.Values["controller"]?.ToString() ?? "";
            string actionName = context.RouteData.Values["action"]?.ToString() ?? "";
            throw new UserActivityLogException(actionName, controller, areaName);
        }


        // OPTIONAL: Set version truncation to none, so full versions will be returned
        // By default only minor versions will be returned (e.g. X.Y)
        // for other options see VERSION_TRUNCATION_* constants in DeviceParserAbstract class
        // add using DeviceDetectorNET.Parser;
        DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);

        var userAgent = context.HttpContext.Request.Headers.UserAgent.ToString();
        var headers =
            context.HttpContext.Request.Headers.ToDictionary(a => a.Key, a => a.Value.ToArray().FirstOrDefault());
        var clientHints = ClientHints.Factory(headers); // client hints are optional

        var dd = new DeviceDetector(userAgent, clientHints);

        // OPTIONAL: Set caching method
        // By default static cache is used, which works best within one php process (memory array caching)
        // To cache across requests use caching in files or memcache
        // add using DeviceDetectorNET.Cache;
        dd.SetCache(new DictionaryCache());

        // OPTIONAL: If called, GetBot() will only return true if a bot was detected  (speeds up detection a bit)
        dd.DiscardBotInformation();


        dd.Parse();
        
        
        Dictionary<string, string> formData = new();
        try
        {
            foreach (var item in context.HttpContext.Request.Form.Keys)
            {
                formData.Add(item, context.HttpContext.Request.Form[item]);   
            }
        }catch{}
        
        InsertUserActivityViewModel userActivity = new();
        userActivity.Description = userActivityLogAttribute?.Description;;
        userActivity.IpAddress = context.HttpContext.GetUserIpAddress();;
        userActivity.UserId = context.HttpContext.User.GetUserId();
        userActivity.Area = context.RouteData.Values["area"]?.ToString() ?? "No-Area";
        userActivity.Url =  context.HttpContext.Request.GetDisplayUrl();
        userActivity.StatusCode = context.HttpContext.Response.StatusCode;
        userActivity.FormData = formData.ToJson();;
        
        
        if (dd.IsBot())
        {
            // handle bots,spiders,crawlers,...
            var botInfo = dd.GetBot();
            userActivity.IsBot = false;
            userActivity.BotCategory = botInfo.Match.Category;
            userActivity.BotProducer = botInfo.Match.Producer.Name;
            userActivity.BotName = botInfo.Match.Name;
            userActivity.BotUrl = botInfo.Match.Url;
        }
        else
        {
            var clientInfo = dd.GetClient();
            var osInfo = dd.GetOs();
            var device = dd.GetDeviceName();
            var brand = dd.GetBrandName();
            var model = dd.GetModel();

            userActivity.IsBot = false;
            userActivity.Brand = brand;
            userActivity.Device = device;
            userActivity.Os = osInfo.Match.Name;
            userActivity.OsPlatform = osInfo.Match.Platform;
            userActivity.OsVersion = osInfo.Match.Version;
            userActivity.Model = model;
            userActivity.BrowserVersion = clientInfo.Match.Version;
            userActivity.BrowserName = clientInfo.Match.Name;
        }

        userActivityService.InsertAsync(userActivity);
    }
}