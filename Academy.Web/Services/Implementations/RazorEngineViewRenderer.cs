using Academy.Web.Services.Interfaces;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;

namespace Academy.Web.Services.Implementations;

public class RazorEngineViewRenderer(
    IHttpContextAccessor httpContextAccessor,
    IRazorViewEngine razorViewEngine,
    ITempDataProvider tempDataProvider,
    IServiceProvider serviceProvider,
    IOptions<MvcViewOptions> viewOptions,
    IWebHostEnvironment webHostEnvironment , 
    IHttpClientFactory httpClientFactory , 
    ILogger<RazorEngineViewRenderer> logger) : IRazorEngineViewRenderer
{
    public async Task<string> RenderRazorViewAsync<T>(T model, string viewPath , bool convertToInlineCss = true, bool convertToInlineJs = true)
    {
        ActionContext actionContext = GetActionContext();
        IView view = FindView(actionContext, viewPath);
        string serverPath = webHostEnvironment.ContentRootPath;
        string htmlString;
        using (StringWriter sw = new StringWriter())
        {
            ActionContext actionContext1 = actionContext;
            IView view1 = view;
            ViewDataDictionary<T> viewData =
                new ViewDataDictionary<T>(new EmptyModelMetadataProvider(), new ModelStateDictionary());
            viewData.Model = model;
            TempDataDictionary tempData = new TempDataDictionary(actionContext.HttpContext, tempDataProvider);
            StringWriter writer = sw;
            HtmlHelperOptions htmlHelperOptions = viewOptions.Value.HtmlHelperOptions;
            ViewContext viewContext =
                new ViewContext(actionContext1, view1, viewData, tempData, writer, htmlHelperOptions);
            await view.RenderAsync(viewContext);
            string renderedHtml = sw.ToString();
            htmlString = ConvertRelativePathsToAbsolute(renderedHtml);
        }

        actionContext = null;
        view = null;
        serverPath = null;
        
        if(convertToInlineCss) 
            htmlString = await ConvertToInternalCss(htmlString);
        
        if(convertToInlineJs)
            htmlString = await ConvertToInternalJavascript(htmlString);
        
        
        return htmlString;
    }

    private string ConvertRelativePathsToAbsolute(string html)
    {
        HtmlDocument htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);    
        
        HttpContext httpContext = httpContextAccessor?.HttpContext ?? new DefaultHttpContext
        {
            RequestServices = serviceProvider
        };
        
        var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
        
        
        foreach (HtmlNode img in htmlDocument.DocumentNode.Descendants("img"))
        {
            HtmlAttribute srcAttr = img.Attributes["src"];
            if (srcAttr != null && !IsAbsoluteUrl(srcAttr.Value))
            {
                string webPath = "/" + srcAttr.Value.TrimStart('/').Replace("\\", "/");
                srcAttr.Value = baseUrl + webPath;
            }
        }


        return htmlDocument.DocumentNode.OuterHtml;
    }

    private async Task<string> ConvertToInternalCss(string html)
    {
        var client = httpClientFactory.CreateClient("IgnoreSsl");
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        HttpContext httpContext = httpContextAccessor?.HttpContext ?? new DefaultHttpContext
        {
            RequestServices = serviceProvider
        };

        var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";

        var linkNodes = htmlDocument.DocumentNode
            .SelectNodes("//link[@rel='stylesheet'][@href]") ?? Enumerable.Empty<HtmlNode>();

        foreach (var link in linkNodes.ToList())
        {
            var href = link.GetAttributeValue("href", "");
            try
            {
                var cssUri = Uri.TryCreate(href, UriKind.Absolute, out var absoluteUri)
                    ? absoluteUri
                    : new Uri(new Uri(baseUrl), href);

                var cssContent = await client.GetStringAsync(cssUri);

                var styleNode = HtmlNode.CreateNode("<style></style>");
                styleNode.RemoveAllChildren();
                styleNode.AppendChild(htmlDocument.CreateTextNode(cssContent));

                link.ParentNode.InsertBefore(styleNode, link);
                link.Remove();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to parse CSS from {Href}", href);
            }
        }

        return htmlDocument.DocumentNode.OuterHtml;
    }
    
    
    private async Task<string> ConvertToInternalJavascript(string html)
    {
        var client = httpClientFactory.CreateClient("IgnoreSsl");
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);
        
        HttpContext httpContext = httpContextAccessor?.HttpContext ?? new DefaultHttpContext
        {
            RequestServices = serviceProvider
        };
        
        var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";

        foreach (var script in htmlDocument.DocumentNode.SelectNodes("//script[@src]") ?? Enumerable.Empty<HtmlNode>())
        {
            var src = script.GetAttributeValue("src", "");
            try
            {
                Uri scriptUri = Uri.TryCreate(src, UriKind.Absolute, out var absoluteUri)
                    ? absoluteUri
                    : new Uri(new Uri(baseUrl), src);

                var js = await client.GetStringAsync(scriptUri);
                script.Attributes.Remove("src");
                script.RemoveAllChildren();
                script.AppendChild(htmlDocument.CreateTextNode(js));
            }
            catch(Exception e)
            {
                logger.LogError(e, "Unable to parse Javascript");
            }
        }

        return htmlDocument.DocumentNode.OuterHtml;
    }

    private bool IsAbsoluteUrl(string url) => Uri.TryCreate(url, UriKind.Absolute, out Uri _);

    private IView FindView(ActionContext actionContext, string viewPath)
    {
        ViewEngineResult view1 = razorViewEngine.GetView(null, viewPath, true);
        if (view1.Success)
            return view1.View;
        ViewEngineResult view2 = razorViewEngine.FindView(actionContext, viewPath, true);
        if (view2.Success)
            return view2.View;
        IEnumerable<string> second = view1.SearchedLocations.Concat(view2.SearchedLocations);
        throw new InvalidOperationException(string.Join(Environment.NewLine, new[]
        {
            "Unable to find view '" + viewPath + "'. The following locations were searched:"
        }.Concat(second)));
    }

    private ActionContext GetActionContext()
    {
        HttpContext httpContext = httpContextAccessor?.HttpContext ?? new DefaultHttpContext
        {
            RequestServices = serviceProvider
        };
        return new ActionContext(httpContext, httpContext.GetRouteData(), new ActionDescriptor());
    }
}