using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Academy.Web.Helpers.TagHelpers;

public class BreadcrumbTagHelper : TagHelper
{
    private const string PageTitleAttributeName = "page-title";

    [HtmlAttributeName(PageTitleAttributeName)]
    public string PageTitle { get; set; }

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var cc = await output.GetChildContentAsync();

        output.TagName = "div";
        output.Attributes.SetAttribute("class", "breadcrumbarea");

        // گرفتن Area فعلی از RouteData
        var area = ViewContext.RouteData.Values["area"]?.ToString();

        string homeUrl = string.IsNullOrWhiteSpace(area) ? "/" : $"/{area}";
        string homeItem = $"""
                               <li>
                                   <a href="{homeUrl}">خانه</a>
                               </li>
                           """;

        string breadcrumbItems = homeItem + cc.GetContent();

        output.Content.SetHtmlContent(
            $@"<div class=""container"">
                <div class=""row"">
                    <div class=""col-xl-12"">
                        <div class=""breadcrumb__content__wraper"" data-aos=""fade-up"">
                            <div class=""breadcrumb__inner"">
                                 <ul>{breadcrumbItems}</ul>
                            </div>
                            <div class=""breadcrumb__title"">
                                <h2 class=""heading"">{PageTitle}</h2>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class=""shape__icon__2"">
                <img loading=""lazy"" class=""shape__icon__img shape__icon__img__1"" src=""/shared/img/herobanner/herobanner__1.png"" alt=""photo"">
                <img loading=""lazy"" class=""shape__icon__img shape__icon__img__2"" src=""/shared/img/herobanner/herobanner__2.png"" alt=""photo"">
                <img loading=""lazy"" class=""shape__icon__img shape__icon__img__3"" src=""/shared/img/herobanner/herobanner__3.png"" alt=""photo"">
                <img loading=""lazy"" class=""shape__icon__img shape__icon__img__4"" src=""/shared/img/herobanner/herobanner__5.png"" alt=""photo"">
            </div>");
    }
}
