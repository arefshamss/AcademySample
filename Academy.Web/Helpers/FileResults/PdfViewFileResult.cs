using Academy.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Helpers.FileResults;

public class PdfViewFileResult : IActionResult
{
    private readonly object? _model;
    private readonly string? _viewPath;
    
    
    public PdfViewFileResult()
    {
        
    }

    public PdfViewFileResult(string viewPath)
    {
        _viewPath = viewPath;
    }

    
    public PdfViewFileResult(string viewPath , object? model)
    {
        _model = model;
        _viewPath = viewPath;
    }
    
    public PdfViewFileResult(object model)
    {
        _model = model;
    }
    
    public async Task ExecuteResultAsync(ActionContext context)
    {
        var razorEngineViewRenderer =  context.HttpContext.RequestServices.GetRequiredService<IRazorEngineViewRenderer>();
        ChromePdfRenderer renderer = new ChromePdfRenderer();

        renderer.RenderingOptions.MarginBottom = 10;
        renderer.RenderingOptions.MarginLeft = 10;
        renderer.RenderingOptions.MarginRight = 10;
        renderer.RenderingOptions.MarginTop = 10;
        renderer.RenderingOptions.EnableJavaScript = true;
        
        var viewPath = string.IsNullOrEmpty(_viewPath?.Trim()) ? context.GetDefaultViewPath() : _viewPath;
        var html = await razorEngineViewRenderer.RenderRazorViewAsync(_model, viewPath);
        PdfDocument pdf = await renderer.RenderHtmlAsPdfAsync(html);
        context.HttpContext.Response.Headers.Append("Content-Disposition", "inline");
        await  context.HttpContext.Response.Body.WriteAsync(pdf.BinaryData, 0, pdf.BinaryData.Length);
    }
}