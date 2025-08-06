using Academy.Domain.Models.EmailSmtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academy.Data.Configurations.EmailSmtpConfigs;

public class EmailSmtpConfig:IEntityTypeConfiguration<EmailSmtp>
{
    public void Configure(EntityTypeBuilder<EmailSmtp> builder)
    {
        #region Key

        builder.HasKey(s => s.Id);

        #endregion

        #region Validation

        builder.Property(s => s.DisplayName).HasMaxLength(100);
        builder.Property(s => s.EamilAddress).HasMaxLength(400);
        builder.Property(s => s.Password).HasMaxLength(400);
        builder.Property(s => s.SmtpAddress).HasMaxLength(400);

        #endregion

    }
}