using Academy.Web.Extensions;

namespace Academy.Web.Configurations;

internal static class TusChunkConfigurationStatic
{
    internal static TusChunkConfigurations TusChunkConfigurations { get; } = new()
    {
        ExpirationType = ExpirationType.Absolute ,
        ChunksExpirationDuration = TimeSpan.FromDays(7) ,
        EndpointPath = "/chunked-files" ,
        DefaultUploadPath = "Content/ChuckUploads/Final" , 
        UploadChunksPath = "Content/ChuckUploads/Parts" ,
        MaxAllowedUploadSizeInMegaByte = 1024 , 
    };
}