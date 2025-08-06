using Academy.Domain.Models.SiteSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academy.Data.Configurations.SiteSetting;

public class SiteSettingConfig : IEntityTypeConfiguration<SiteSettings>
{
    public void Configure(EntityTypeBuilder<SiteSettings> builder)
    {
        #region Key
        
        builder.HasKey(s => s.Id);

        #endregion
        
        #region Validations
        
        builder.Property(s => s.Favicon)
            .HasMaxLength(200);
        
        builder.Property(s => s.Logo)
            .HasMaxLength(200);

        builder.Property(s => s.Copyright)
            .HasMaxLength(400);
        
        builder.Property(s => s.SiteName)
            .HasMaxLength(100);

        #endregion

        #region Relations

        builder.HasOne(s => s.DefaultMailServerSmtp)
            .WithMany(s => s.MailServerSmtpSiteSettings)
            .HasForeignKey(s => s.DefaultMailServerSmtpId);

        builder.HasOne(s => s.DefaultSimpleSmtp)
            .WithMany(s => s.SimpleSmtpSiteSettings)
            .HasForeignKey(s => s.DefaultSimpleSmtpId);

        #endregion
    }
}