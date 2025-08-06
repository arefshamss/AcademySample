using Academy.Application.Statics;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Helpers.FileResults;

public class CsvFileResult(byte[] fileContents) : IActionResult
{
    private string _fileName;

    public Task ExecuteResultAsync(ActionContext context)
    {
        string cookieValue = Guid.NewGuid().ToString();
        context.HttpContext.Response.Cookies.Append("FileDownload", cookieValue, new CookieOptions()
        {
            Expires = DateTime.Now.AddMinutes(1)
        });
        _fileName = FileGeneratorStatics.CsvFileName;
        var response = context.HttpContext.Response;
        response.Headers.Add("Content-Disposition", $"attachment; filename=\"{_fileName}\"");
        response.ContentType = FileGeneratorStatics.CsvContentType;
        return response.Body.WriteAsync(fileContents, 0, fileContents.Length);
    }
}