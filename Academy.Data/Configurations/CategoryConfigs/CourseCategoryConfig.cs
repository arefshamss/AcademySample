using Academy.Domain.Models.Category;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academy.Data.Configurations.CategoryConfigs;

public class CourseCategoryConfig : IEntityTypeConfiguration<CourseCategory>
{
    public void Configure(EntityTypeBuilder<CourseCategory> builder)
    {
        #region Key

        builder.HasKey(x => x.Id);

        #endregion

        #region Validations

        builder.Property(x => x.Title)
            .HasMaxLength(150)
            .IsRequired();


        builder.Property(x => x.Slug)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.ParentId)
            .HasMaxLength(10)
            .IsRequired(false);

        builder.Property(x => x.Priority)
            .HasMaxLength(10)
            .IsRequired();
        
        #endregion

        #region Relations

        builder.HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
        
        #endregion
    }
}