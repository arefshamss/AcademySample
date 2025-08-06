using System.Text;
using Academy.Application.Statics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Academy.Web.Helpers.FileResults;

public class JsonFileResult<TValue>(TValue value) : IActionResult
{
    private string _fileName;

    public Task ExecuteResultAsync(ActionContext context)
    {
        string cookieValue = Guid.NewGuid().ToString();
        context.HttpContext.Response.Cookies.Append("FileDownload", cookieValue, new CookieOptions()
        {
            Expires = DateTime.Now.AddMinutes(1)
        });
        _fileName = FileGeneratorStatics.JsonFileName;
        var response = context.HttpContext.Response;
        using var writer = new StringWriter();
        using var jsonWriter = new JsonTextWriter(writer);
        // Create a new instance of JsonSerializer (non-static)
        var serializer = new JsonSerializer();
        serializer.Serialize(jsonWriter, value);

        // Get the JSON string with actual characters
        string jsonString = writer.ToString();

        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);

        response.Headers.Add("Content-Disposition", $"attachment; filename=\"{_fileName}\"");
        response.ContentType = FileGeneratorStatics.JsonContentType;
        return response.Body.WriteAsync(jsonBytes, 0, jsonBytes.Length);
    }
}