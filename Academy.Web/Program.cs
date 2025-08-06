using Academy.Web.Extensions;
using Serilog;
try
{
    WebApplication.CreateBuilder(args)
        .AddApplicationServices()
        .UseApplicationPipelines();
}
catch (Exception ex)
{
    Log.Error($"Stopped program because of exception \n exception is : {ex.Message}  \n stack trace : {ex.StackTrace}");
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}
