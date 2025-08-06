using Ganss.Xss;

namespace Academy.Application.Extensions;

public static class XssSecurity
{
    public static string SanitizeText(this string text)
    {
        HtmlSanitizer htmlSanitizer = new() 
        {
            KeepChildNodes = true,
            AllowDataAttributes = true
        };
    
        return htmlSanitizer.Sanitize(text);
    }
    
    public static string SanitizeTextAndTrim(this string text)
    {
        return text.Trim().SanitizeText();
    }
}