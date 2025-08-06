using Academy.Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academy.Data.Configurations.UserConfigs;

public class UserInformationConfig : IEntityTypeConfiguration<UserInformation>
{
    public void Configure(EntityTypeBuilder<UserInformation> builder)
    {
        #region Key

        builder.HasKey(x => x.Id);

        #endregion

        #region Validations

        builder.Property(x => x.BirthDate)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(x => x.PostalCode)
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(x => x.FatherName)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.Address)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(x => x.NationalCode)
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(x => x.BirthCertificateNumber)
            .HasMaxLength(20)
            .IsRequired(false);

        #endregion
        
        #region Relations

        builder.HasOne(x => x.User)
            .WithOne(x => x.UserInformation)
            .HasForeignKey<UserInformation>(x => x.UserId)
            .IsRequired();

        builder.HasIndex(x => x.UserId).IsUnique();

        #endregion
    }
}