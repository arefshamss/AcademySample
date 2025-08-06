using Academy.Domain.Models.SiteSettings;

namespace Academy.Data.Seeds;

public static class SiteSettingSeeds
{
    public static List<SiteSettings> SiteSettingsList { get; } =
    [
        new()
        {
            Id = 1,
            Copyright = "تمامی حقوق مادی و معنوی این سایت متعلق به آکادمی می باشد و هرگونه کپی برداری غیرقانونی محسوب خواهد شد",
            Favicon = "favicon.ico",
            Logo = "logo.png" , 
            IsDeleted = false,
            SiteName = "آکادمی" ,
            CreatedDate = DateTime.Now,
        }
    ];
}