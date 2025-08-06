using Academy.Application.Extensions;
using Academy.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Academy.Web.Helpers.TagHelpers;

/// <summary>
///     filters result of output based on the user permission. if the user don,t have the permission to see the result than the user won,t see it 
/// </summary>
[HtmlTargetElement("*", Attributes = "invoke-permission")]
public class PermissionTagHelper(IPermissionService permissionService) : TagHelper
{
    [ViewContext] [HtmlAttributeNotBound] public ViewContext ViewContextData { get; set; }

    /// <summary>
    /// permissions unique name to check for the user access. you can pass multiple parameters by separating them using a pipe "|"
    /// </summary>
    [HtmlAttributeName("invoke-permission")]
    public string PermissionName { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        // disabled 
        return;
        
        
        if (!ViewContextData.HttpContext.User.Identity.IsAuthenticated)
        {
            output.SuppressOutput();
            return;
        }

        var userId = ViewContextData.HttpContext.User.GetUserId();
        if (userId < 1 || userId == null)
        {
            output.SuppressOutput();
            return;
        }

        var permissions = PermissionName
            .Split("|")
            .Select(n => n.Trim())
            .ToList();

        if (permissions == null || !permissions.Any())
            return;


        if (await permissionService.CheckUserPermissionsAsync(userId, permissions)) return;
        output.SuppressOutput();
    }
}