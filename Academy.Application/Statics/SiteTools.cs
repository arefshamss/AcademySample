namespace Academy.Application.Statics;

public class SiteTools
{
    
    #region DefaultNames

    public static string DefaultImageName { get; set; }
    
    public static string DefaultGifName { get; set; }

    #endregion
    
    #region Data protection

    public static string keyDirectoryServerPath { get; set; } =
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/KeyDirectory/");

    #endregion

}