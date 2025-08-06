namespace Academy.Web.Services.Interfaces;

public interface IRazorEngineViewRenderer
{
    public Task<string> RenderRazorViewAsync<T>(T model, string viewPath  , bool convertToInlineCss = true, bool convertToInlineJs = true);
}