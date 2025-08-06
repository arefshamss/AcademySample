using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Academy.Web.Helpers.TagHelpers;

public class BreadcrumbItemTagHelper : TagHelper
{
    private const string ActionAttributeName = "asp-action";
    private const string ControllerAttributeName = "asp-controller";
    private const string AreaAttributeName = "asp-area";
    private const string PageAttributeName = "asp-page";
    private const string PageHandlerAttributeName = "asp-page-handler";
    private const string FragmentAttributeName = "asp-fragment";
    private const string HostAttributeName = "asp-host";
    private const string ProtocolAttributeName = "asp-protocol";
    private const string RouteAttributeName = "asp-route";
    private const string RouteValuesDictionaryName = "asp-all-route-data";
    private const string RouteValuesPrefix = "asp-route-";
    private const string Href = "href";
    private const string LastActiveItem = "last-active-item";

    private IDictionary<string, string> _routeValues;

    /// <summary>
    /// Creates a new <see cref="AnchorTagHelper"/>.
    /// </summary>
    /// <param name="generator">The <see cref="IHtmlGenerator"/>.</param>
    public BreadcrumbItemTagHelper(IHtmlGenerator generator)
    {
        Generator = generator;
    }

    /// <inheritdoc />
    public override int Order => -1000;

    /// <summary>
    /// Gets the <see cref="IHtmlGenerator"/> used to generate the <see cref="AnchorTagHelper"/>'s output.
    /// </summary>
    protected IHtmlGenerator Generator { get; }

    /// <summary>
    /// The name of the action method.
    /// </summary>
    /// <remarks>
    /// Must be <c>null</c> if <see cref="Route"/> or <see cref="Page"/> is non-<c>null</c>.
    /// </remarks>
    [HtmlAttributeName(ActionAttributeName)]
    public string Action { get; set; }

    /// <summary>
    /// The name of the controller.
    /// </summary>
    /// <remarks>
    /// Must be <c>null</c> if <see cref="Route"/> or <see cref="Page"/> is non-<c>null</c>.
    /// </remarks>
    [HtmlAttributeName(ControllerAttributeName)]
    public string Controller { get; set; }

    /// <summary>
    /// The name of the area.
    /// </summary>
    /// <remarks>
    /// Must be <c>null</c> if <see cref="Route"/> is non-<c>null</c>.
    /// </remarks>
    [HtmlAttributeName(AreaAttributeName)]
    public string Area { get; set; }

    /// <summary>
    /// The name of the page.
    /// </summary>
    /// <remarks>
    /// Must be <c>null</c> if <see cref="Route"/> or <see cref="Action"/>, <see cref="Controller"/>
    /// is non-<c>null</c>.
    /// </remarks>
    [HtmlAttributeName(PageAttributeName)]
    public string Page { get; set; }

    /// <summary>
    /// The name of the page handler.
    /// </summary>
    /// <remarks>
    /// Must be <c>null</c> if <see cref="Route"/> or <see cref="Action"/>, or <see cref="Controller"/>
    /// is non-<c>null</c>.
    /// </remarks>
    [HtmlAttributeName(PageHandlerAttributeName)]
    public string PageHandler { get; set; }

    /// <summary>
    /// The protocol for the URL, such as &quot;http&quot; or &quot;https&quot;.
    /// </summary>
    [HtmlAttributeName(ProtocolAttributeName)]
    public string Protocol { get; set; }

    /// <summary>
    /// The host name.
    /// </summary>
    [HtmlAttributeName(HostAttributeName)]
    public string Host { get; set; }

    /// <summary>
    /// The URL fragment name.
    /// </summary>
    [HtmlAttributeName(FragmentAttributeName)]
    public string Fragment { get; set; }

    /// <summary>
    /// Name of the route.
    /// </summary>
    /// <remarks>
    /// Must be <c>null</c> if one of <see cref="Action"/>, <see cref="Controller"/>, <see cref="Area"/>
    /// or <see cref="Page"/> is non-<c>null</c>.
    /// </remarks>
    [HtmlAttributeName(RouteAttributeName)]
    public string Route { get; set; }

    /// <summary>
    /// Additional parameters for the route.
    /// </summary>
    [HtmlAttributeName(RouteValuesDictionaryName, DictionaryAttributePrefix = RouteValuesPrefix)]
    public IDictionary<string, string> RouteValues
    {
        get
        {
            if (_routeValues == null)
            {
                _routeValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }

            return _routeValues;
        }
        set { _routeValues = value; }
    }

    /// <summary>
    /// Gets or sets the <see cref="Microsoft.AspNetCore.Mvc.Rendering.ViewContext"/> for the current request.
    /// </summary>
    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }


    /// <summary>
    /// if its the last active item in the breadcrumb it gets the class active and no link attached to it  
    /// </summary>
    [HtmlAttributeName(LastActiveItem)]
    public bool LastActiveLink { get; set; }

    /// <inheritdoc />
    /// <remarks>Does nothing if user provides an <c>href</c> attribute.</remarks>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(output);

        var childContent = await output.GetChildContentAsync();

        if (LastActiveLink)
        {
            output.TagName = "li";
            output.Attributes.Clear();
            output.Content.SetHtmlContent(childContent.GetContent());
            return;
        }
    
        if (output.Attributes.ContainsName(Href))
        {
            output.TagName = "li";
            output.Content.SetHtmlContent($"<a href=\"{output.Attributes[Href].Value}\">{childContent.GetContent()}</a>");
            return;
        }
    
        var hasRouteValues = !string.IsNullOrEmpty(Action) ||
                             !string.IsNullOrEmpty(Controller) ||
                             !string.IsNullOrEmpty(Area) ||
                             !string.IsNullOrEmpty(Page) ||
                             !string.IsNullOrEmpty(PageHandler) ||
                             !string.IsNullOrEmpty(Route) ||
                             !string.IsNullOrEmpty(Protocol) ||
                             !string.IsNullOrEmpty(Host) ||
                             !string.IsNullOrEmpty(Fragment) ||
                             (_routeValues != null && _routeValues.Count > 0);

        if (!hasRouteValues)
        {
            output.TagName = "li";
            output.Content.SetHtmlContent(childContent.GetContent());
            return;
        }

        RouteValueDictionary routeValues = null;
        if (_routeValues != null && _routeValues.Count > 0)
        {
            routeValues = new RouteValueDictionary(_routeValues);
        }

        if (Area != null)
        {
            if (routeValues == null)
            {
                routeValues = new RouteValueDictionary();
            }
            routeValues["area"] = Area;
        }

        TagBuilder tagBuilder;

        if (!string.IsNullOrEmpty(Page) || !string.IsNullOrEmpty(PageHandler))
        {
            tagBuilder = Generator.GeneratePageLink(
                ViewContext,
                linkText: string.Empty,
                pageName: Page,
                pageHandler: PageHandler,
                protocol: Protocol,
                hostname: Host,
                fragment: Fragment,
                routeValues: routeValues,
                htmlAttributes: null);
        }
        else if (!string.IsNullOrEmpty(Route))
        {
            tagBuilder = Generator.GenerateRouteLink(
                ViewContext,
                linkText: string.Empty,
                routeName: Route,
                protocol: Protocol,
                hostName: Host,
                fragment: Fragment,
                routeValues: routeValues,
                htmlAttributes: null);
        }
        else
        {
            tagBuilder = Generator.GenerateActionLink(
                ViewContext,
                linkText: string.Empty,
                actionName: Action,
                controllerName: Controller,
                protocol: Protocol,
                hostname: Host,
                fragment: Fragment,
                routeValues: routeValues,
                htmlAttributes: null);
        }

        var url = tagBuilder.Attributes["href"];

        output.TagName = "li";
        output.Content.SetHtmlContent($"<a href=\"{url}\">{childContent.GetContent()}</a>");
    }

}