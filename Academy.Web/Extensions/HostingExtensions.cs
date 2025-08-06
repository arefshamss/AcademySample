using System.Text.Encodings.Web;
using System.Text.Unicode;
using Academy.Web.ActionFilters;
using Academy.Web.Configurations;
using Academy.Web.Middlewares;
using Academy.Web.Services.Implementations;
using Academy.Web.Services.Interfaces;
using Academy.Application.Cache;
using Academy.Application.Statics;
using Academy.Data.Context;
using Academy.IOC;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using OfficeOpenXml;
using Serilog;
using Serilog.Events;
using tusdotnet.Interfaces;
using tusdotnet.Stores;

namespace Academy.Web.Extensions;

internal static class HostingExtensions
{
    #region Serivces

    internal static WebApplication AddApplicationServices(this WebApplicationBuilder builder)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        
        builder.Services.AddHttpContextAccessor();
        
        string mongoConnectionString = builder.Configuration.GetConnectionString("AcademyMongoDBConnectionString") ?? throw new NullReferenceException();

            string mongoDbName = builder.Configuration.GetConnectionString("AcademyMongoDBDatabaseName") ?? throw new NullReferenceException();

            builder.Services
                .AddControllersWithViews()
                .AddMvcOptions(options =>
                {
                    options.Filters.Add<UserActivityLogFilter>();
                    options.Filters.Add<SanitizeActionFilter>();
                });

            builder.Configuration.GetSection("SiteTools").Get<SiteTools>();
            builder.Configuration.GetSection("GoogleAuth").Get<GoogleAuth>();
            builder.Configuration.GetSection("FilePaths").Get<FilePaths>();

            builder.Services.AddSingleton<TusChunkConfigurations>(TusChunkConfigurationStatic.TusChunkConfigurations);
            builder.Services.AddScoped<IRazorEngineViewRenderer , RazorEngineViewRenderer>();
            builder.Services.AddSingleton<ITusStore>(sp =>
            {
                var tusStorePath = Path.Combine(Directory.GetCurrentDirectory(), TusChunkConfigurationStatic.TusChunkConfigurations.UploadChunksPath);
                return new TusDiskStore(tusStorePath);
            });


            builder.Services.AddEasyCaching(options => options.UseInMemory(CacheProviders.InMemoryCachingProviderName))
                .AddApplicationAuthentication()
                .AddApplicationJobs();

            #region Data protection

             builder.Services.AddDataProtection()
                 .PersistKeysToFileSystem(new DirectoryInfo(SiteTools.keyDirectoryServerPath))
                 .SetApplicationName("Academy")
                 .SetDefaultKeyLifetime((TimeSpan.FromDays(30)));

            #endregion

            #region Add db context


            builder.Services.AddDbContext<AcademyContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("AcademyConnectionString"));
            });


            var mongoClient =
                new MongoClient(mongoConnectionString)
                    .GetDatabase(mongoDbName);

            builder.Services.AddDbContext<AcademyMongoDbContext>(options =>
            {
                options.UseMongoDB(mongoClient.Client, mongoClient.DatabaseNamespace.DatabaseName);
            });

            #endregion

            #region Config encoder

            builder.Services.AddSingleton(
                HtmlEncoder.Create(allowedRanges: new[]
                {
                    UnicodeRanges.BasicLatin,
                    UnicodeRanges.Arabic
                }));

            #endregion


            #region Config serilog

            builder.Logging.ClearProviders().AddSerilog();
            
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithMachineName()
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(s => s.Level == LogEventLevel.Information)
                    .WriteTo.File(
                        path: "Logs/log-info-.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                )
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(s => s.Level == LogEventLevel.Warning)
                    .WriteTo.File(
                        path: "Logs/log-warning-.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                )
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(s => s.Level == LogEventLevel.Error)
                    .WriteTo.File(
                        path: "Logs/log-error-.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                )
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(s => s.Level == LogEventLevel.Fatal)
                    .WriteTo.File(
                        path: "Logs/log-fatal-.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                )
                .WriteTo.MongoDB(
                    database: mongoClient,
                    collectionName: "ApplicationLogs")
                .CreateLogger();

            Serilog.Debugging.SelfLog.Enable(Console.WriteLine);

            builder.Host.UseSerilog();
                
            #endregion

            builder.Services.AddCookiePolicy(options => { options.Secure = CookieSecurePolicy.Always; });

            builder.Services.AddSession();
            builder.Services.RegisterServices()
                .AddHttpClientFactories();

            builder.Services.AddResumingFileResult();

            builder.Services.AddHttpClient();

        return builder.Build();
    }

    #endregion

    #region Pipelines

    internal static void UseApplicationPipelines(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/server-error");
            app.UseHsts();
        }
        else
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSession();

        app.UseHttpsRedirection();

        app.UseMiddleware<ExceptionHandlerMiddleware>();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseTusChunkUpload(cfg =>
        {
            cfg.ExpirationType = TusChunkConfigurationStatic.TusChunkConfigurations.ExpirationType;
            cfg.ChunksExpirationDuration = TusChunkConfigurationStatic.TusChunkConfigurations.ChunksExpirationDuration;
            cfg.EndpointPath = TusChunkConfigurationStatic.TusChunkConfigurations.EndpointPath;
            cfg.DefaultUploadPath = TusChunkConfigurationStatic.TusChunkConfigurations.DefaultUploadPath;
            cfg.UploadChunksPath = TusChunkConfigurationStatic.TusChunkConfigurations.UploadChunksPath;
            cfg.MaxAllowedUploadSizeInMegaByte = TusChunkConfigurationStatic.TusChunkConfigurations.MaxAllowedUploadSizeInMegaByte;
        });

        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
            
            
        app.Run();

    }

    #endregion

}