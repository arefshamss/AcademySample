using Academy.Domain.Models.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academy.Data.Configurations.RoleConfigs;

public class RoleConfig:IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        #region Key

        builder.HasKey(s => s.Id);

        #endregion

        #region Validation

        builder.Property(s => s.RoleName).HasMaxLength(300);

        #endregion

        #region Relations

        builder.HasMany(s => s.UserRoleMappings)
            .WithOne(s => s.Role)
            .HasForeignKey(s => s.RoleId);

        builder.HasMany(s => s.RolePermissionMappings)
            .WithOne(s => s.Role)
            .HasForeignKey(s => s.RoleId);

        #endregion

    }
}