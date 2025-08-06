using Newtonsoft.Json;

namespace Academy.Web.Extensions;

public static class HttpContextExtensions
{
    public static T? GetCookie<T>(this HttpContext context, string cookieKey)
    {
        var dataString = context.Request.Cookies[cookieKey];

        if (string.IsNullOrEmpty(dataString))
            return default;

        T? data = JsonConvert.DeserializeObject<T>(dataString);

        return data;
    }

    public static void SetCookie(this HttpContext context, string cookieKey, object data)
    {
        CookieOptions option = new CookieOptions();

        option.Expires = DateTime.Now.AddDays(90);

        string dataString = JsonConvert.SerializeObject(data);

        context.Response.Cookies.Append(cookieKey, dataString, option);
    }

    public static void DeleteCookie(this HttpContext context, string cookieKey)
    {
        CookieOptions option = new CookieOptions();

        option.Expires = DateTime.Now.AddDays(-90);

        context.Response.Cookies.Append(cookieKey, "", option);
    }

    public static string GetUserIpAddress(this HttpContext httpContext)
    {
        #if DEBUG
                return httpContext.Connection.LocalIpAddress?.ToString() ?? "-";
        #else
                return httpContext.Connection.RemoteIpAddress?.ToString() ?? "-";
        #endif
    }

    public static string GetUrlReferer(this HttpRequest request)
    {
        return request.Headers["Referer"].ToString();
    }
    
    public static string GetOperatingSystem(this HttpContext context)
    {
        var userAgent = context?.Request.Headers["User-Agent"].ToString();
        
        if (userAgent == null)
            return "Unknown";

        if (userAgent.Contains("Windows"))
            return "Windows";
        else if (userAgent.Contains("Macintosh"))
            return "Mac OS";
        else if (userAgent.Contains("Linux"))
            return "Linux";
        else if (userAgent.Contains("Android"))
            return "Android";
        else if (userAgent.Contains("iPhone") || userAgent.Contains("iPad"))
            return "iOS";
        
        return "Unknown";
    }
}