using Academy.Domain.Models.SmsProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academy.Data.Configurations.SmsProviderConfigs;

public class SmsProviderConfig : IEntityTypeConfiguration<SmsProvider>
{
    public void Configure(EntityTypeBuilder<SmsProvider> builder)
    {
        #region Key

        builder.HasKey(s => s.Id);

        #endregion

        #region Validation

        builder.Property(s => s.Title).HasMaxLength(200);
        builder.Property(s => s.ApiKey).HasMaxLength(500);

        #endregion

    }
}