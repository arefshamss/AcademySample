using Academy.Application.Statics;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Helpers.FileResults;

public class ExcelFileResult(byte [] fileContents) : IActionResult
{
    private string _fileName;
    public Task ExecuteResultAsync(ActionContext context)
    {
        string cookieValue = Guid.NewGuid().ToString();
        context.HttpContext.Response.Cookies.Append("FileDownload", cookieValue, new CookieOptions()
        {
            Expires = DateTime.Now.AddMinutes(1)
        });
        _fileName = FileGeneratorStatics.ExcelFileName;
        var response = context.HttpContext.Response;
        response.Headers.Add("Content-Disposition", $"attachment; filename=\"{_fileName}\"");
        response.ContentType = FileGeneratorStatics.ExcelContentType;

        return response.Body.WriteAsync(fileContents, 0, fileContents.Length);
    }
}