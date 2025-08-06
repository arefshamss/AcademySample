using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Academy.Web.Helpers.TagHelpers;

public class AdminBreadcrumbTagHelper : TagHelper
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var cc = await output.GetChildContentAsync();
        output.TagName = "nav";
        output.Attributes.SetAttribute("aria-label", "breadcrumb");
        output.Attributes.SetAttribute("class", "flex flex-1 hidden xl:block");
        output.Content.SetHtmlContent(
            $"<ol class=\"flex items-center text-theme-1 dark:text-slate-300 text-white/90\">\n<li class=\"\">\n<a href=\"/Admin\">پنل مدیریت</a>\n</li>\n{cc.GetContent()}</ol>");
    }
}