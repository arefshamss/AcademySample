namespace Academy.Web.Middlewares;

public class ExceptionHandlerMiddleware(
    ILogger<ExceptionHandlerMiddleware> logger,
    RequestDelegate requestDelegate)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {

        try
        {
            await requestDelegate(httpContext);
            if (!httpContext.Response.HasStarted)
            {
                switch (httpContext.Response.StatusCode)
                {
                    case 400:
                        httpContext.Request.Path = "/bad-request";
                        await requestDelegate(httpContext);
                        break;

                    case 404:
                        httpContext.Request.Path = "/not-found";
                        await requestDelegate(httpContext);
                        break;

                    case 500:
                        httpContext.Request.Path = "/server-error";
                        await requestDelegate(httpContext);
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"error happened while running : {ex.Message}");
            throw;
        }
    }

}