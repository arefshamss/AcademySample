using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academy.Data.Configurations.TicketConfigs;

public class TicketConfig : IEntityTypeConfiguration<Academy.Domain.Models.Ticket.Ticket>
{
    public void Configure(EntityTypeBuilder<Academy.Domain.Models.Ticket.Ticket> builder)
    {
        #region Key

        builder.HasKey(x => x.Id);

        #endregion

        #region Properties

        builder.Property(x => x.Title)
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(x => x.ReadBySupporter)
            .IsRequired();

        builder.Property(x => x.ReadByUser)
            .IsRequired();
        
        builder.Property(x => x.TicketStatus)
            .IsRequired();
        
        #endregion

        #region Relations

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Tickets)
            .HasForeignKey(x => x.UserId);

        #endregion
    }
}