using Academy.Domain.Models.Captcha;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academy.Data.Configurations.CaptchaConfigs;

public class CaptchaConfig : IEntityTypeConfiguration<Captcha>
{
    public void Configure(EntityTypeBuilder<Captcha> builder)
    {
        #region Key

        builder.HasKey(s => s.Id);

        #endregion

        #region Validation

        builder.Property(s => s.Title).HasMaxLength(300);
        builder.Property(s => s.SiteKey).HasMaxLength(500);
        builder.Property(s => s.SecretKey).HasMaxLength(500);

        #endregion

    }
}