using Academy.Domain.Models.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academy.Data.Configurations.PermissionConfigs;

public class PermissionConfig:IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        #region Key

        builder.HasKey(s => s.Id);

        #endregion

        #region Validtion

        builder.Property(s => s.UniqueName).HasMaxLength(400);
        builder.Property(s => s.DisplayName).HasMaxLength(400);

        #endregion

        #region Relations

        builder.HasOne(s => s.Parent)
            .WithMany()
            .HasForeignKey(s => s.ParentId);

        builder.HasMany(s => s.RolePermissionMappings)
            .WithOne(s => s.Permission)
            .HasForeignKey(s => s.PermissionId);

        #endregion

    }
}