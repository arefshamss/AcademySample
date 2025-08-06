using System.Text;
using tusdotnet;
using tusdotnet.Interfaces;
using tusdotnet.Models;
using tusdotnet.Models.Configuration;
using tusdotnet.Models.Expiration;
using tusdotnet.Stores;
using tusdotnet.Stores.FileIdProviders;

namespace Academy.Web.Extensions;
/// <summary>
///  expiration type for incomplete files where can no longer be updated. there are two types. absolute and sliding expiration
/// </summary>
public enum ExpirationType
{
    /// <summary>
    /// Absolute expiration that is only set once during file creation and is never updated after that.
    /// </summary>
    Absolute , 
    /// <summary>
    /// Sliding expiration that set during creation and updated on every PATCH request.
    /// </summary>
    Sliding , 
}


/// <summary>
/// configuration for customizing tusdotnet based on your needs
/// </summary>
public class TusChunkConfigurations
{
    
    /// <summary>
    /// the endpoint path that receives the files sent by client. this should be configured on the client side as well
    /// </summary>
    public string EndpointPath { get; set; } = "/files";

    /// <summary>
    /// the path for saving chunked files. this path is relative from the root of the project and should not have '\' character at the beginning. 
    /// </summary>
    public string UploadChunksPath { get; set; } = "UploadedChunks";
    
    /// <summary>
    ///  default upload path for when the upload path was not set by the client. the completed file will be saved in this path. its relative from the root of project and should not start with the '\' character 
    /// </summary>
    public string? DefaultUploadPath { get; set; }
    
    /// <summary>
    /// The maximum upload size to allow. Exceeding this limit will return a "413 Request Entity Too Large" error to the client. Set to null to allow any size. The size might still be restricted by the web server or operating system
    /// </summary>
    public int? MaxAllowedUploadSizeInMegaByte { get; set; }
    
    /// <summary>
    ///  The time that the incomplete file can live without being flagged as expired
    /// </summary>
    public TimeSpan ChunksExpirationDuration { get; set; } = TimeSpan.FromDays(7);

    
    /// <summary>
    /// set the expiration type for incomplete files where can no longer be updated. there are two types. absolute and sliding expiration
    /// </summary>
    public ExpirationType ExpirationType { get; set; } = ExpirationType.Absolute;

    /// <summary>
    /// The buffer sizes for reads and writes to use with <see cref="TusDiskStore"/>
    /// </summary>
    public TusDiskBufferSize BufferSize { get; set; } = TusDiskBufferSize.Default;
}

internal static class TusChunkUploadExtensions
{
    private static long? ConvertMegabytesToByte(int? megaByteSize)
    {
        if (megaByteSize.HasValue)
        {
            return megaByteSize.Value * 1024 * 1024;
        }

        return default;
    }

    private static void CreateDirectoryIfNotExists(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }


    internal static IApplicationBuilder UseTusChunkUpload(this WebApplication app,
        Action<TusChunkConfigurations>? configurations = default)
    {
        var config = new TusChunkConfigurations();

        configurations?.Invoke(config);

        string tusStorePath = Path.Combine(Directory.GetCurrentDirectory(), config.UploadChunksPath);

        CreateDirectoryIfNotExists(tusStorePath);

        app.MapTus(config.EndpointPath, async httpContext => new DefaultTusConfiguration()
        {
            
            Expiration = config.ExpirationType switch {
                ExpirationType.Absolute => new AbsoluteExpiration(config.ChunksExpirationDuration),
                ExpirationType.Sliding => new SlidingExpiration(config.ChunksExpirationDuration),
                _ => throw new ArgumentOutOfRangeException(nameof(config.ExpirationType), config.ExpirationType, null)
            },
            // Configure storage location
            Store = new TusDiskStore(directoryPath: tusStorePath,
                deletePartialFilesOnConcat: true,
                bufferSize: config.BufferSize,
                fileIdProvider: new GuidFileIdProvider()
            ),
            FileLockProvider = null,
            // Events
            Events = new Events()
            {
                OnBeforeCreateAsync = async ctx => { await Task.CompletedTask; },
                OnCreateCompleteAsync = async ctx => { await Task.CompletedTask; },
                OnFileCompleteAsync = async ctx =>
                {
                    ITusFile file = await ctx.GetFileAsync();

                    var filePath = Path.Combine(config.UploadChunksPath, file.Id);
                    var metaData = await file.GetMetadataAsync(ctx.CancellationToken);
                    metaData.TryGetValue("filename", out var originalFilename);
                    metaData.TryGetValue("suggestedFileName", out var suggestedFileNameMeta);
                    metaData.TryGetValue("location", out var fileLocationMeta);
                    

                    string? fileName = originalFilename?.GetString(Encoding.UTF8);
                    string? suggestedFileName = suggestedFileNameMeta?.GetString(Encoding.UTF8);
                    string? location = fileLocationMeta?.GetString(Encoding.UTF8);

                    string finalFilePath;
                    string fileExtension = Path.GetExtension(fileName);
                    if (!string.IsNullOrEmpty(location))
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), location);
                        CreateDirectoryIfNotExists(path);
                        finalFilePath = !string.IsNullOrEmpty(suggestedFileName)
                            ? Path.Combine(location, suggestedFileName + fileExtension)
                            : Path.Combine(location, fileName);
                    }
                    else if (config.DefaultUploadPath != null)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), config.DefaultUploadPath);
                        CreateDirectoryIfNotExists(path);
                        finalFilePath = !string.IsNullOrEmpty(suggestedFileName)
                            ? Path.Combine(config.DefaultUploadPath, suggestedFileName + fileExtension)
                            : throw  new ArgumentNullException(nameof(suggestedFileName));
                    }
                    else
                    {
                        finalFilePath = !string.IsNullOrEmpty(suggestedFileName)
                            ? Path.Combine(tusStorePath, suggestedFileName + fileExtension)
                            : throw  new ArgumentNullException(nameof(suggestedFileName));
                    }
                    
                    File.Move(filePath, finalFilePath, true);
                    
                    var terminationStore = (ITusTerminationStore)ctx.Store;
                    await terminationStore.DeleteFileAsync(file.Id, ctx.CancellationToken);
                
                    await Task.CompletedTask;
                }
            },
            MaxAllowedUploadSizeInBytesLong = ConvertMegabytesToByte(config.MaxAllowedUploadSizeInMegaByte)
        });

        return app;
    }
}