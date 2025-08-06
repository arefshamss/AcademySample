using Academy.Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academy.Data.Configurations.UserConfigs;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        #region Key

        builder.HasKey(x => x.Id);

        #endregion

        #region Validations

        builder.Property(x => x.FirstName)
            .HasMaxLength(150)
            .IsRequired(false);

        builder.Property(x => x.LastName)
            .HasMaxLength(150)
            .IsRequired(false);

        builder.Property(x => x.Email)
            .HasMaxLength(150)
            .IsRequired(false);

        builder.Property(x => x.Mobile)
            .HasMaxLength(150)
            .IsRequired(false);

        builder.Property(x => x.AvatarImageName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Password)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(x => x.GoogleAuthenticationId)
            .HasMaxLength(150)
            .IsRequired(false);
        
        
        builder.Property(x => x.ActiveCode)
            .HasMaxLength(10)
            .IsRequired(false);
        
        
        builder.Property(x => x.MobileActiveCode)
            .HasMaxLength(10)
            .IsRequired(false);
        
        
        builder.Property(x => x.EmailActiveCode)
            .HasMaxLength(10)
            .IsRequired(false);
        
        #endregion

        #region Relations


        builder.HasMany(s => s.UserRoleMappings)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId);
        
        #endregion
    }
}