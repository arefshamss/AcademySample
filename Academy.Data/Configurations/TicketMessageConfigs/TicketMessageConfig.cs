using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academy.Data.Configurations.TicketMessageConfigs;

public class TicketMessageConfig : IEntityTypeConfiguration<Academy.Domain.Models.Ticket.TicketMessage>
{
    public void Configure(EntityTypeBuilder<Academy.Domain.Models.Ticket.TicketMessage> builder)
    {
        #region Key

        builder.HasKey(x => x.Id);

        #endregion

        #region Properties
        
        builder.Property(x => x.Message)
            .HasMaxLength(1000)
            .IsRequired();
        
        builder.Property(x => x.Attachment)
            .HasMaxLength(150)
            .IsRequired(false);
        
        builder.Property(x => x.ReadBySupporter)
            .IsRequired();
 
        builder.Property(x => x.ReadByUser)
            .IsRequired();

        #endregion

        #region Relations

        builder
            .HasOne(x => x.Ticket)
            .WithMany(x => x.TicketMessages)
            .HasForeignKey(x => x.TicketId);

        builder
            .HasOne(x => x.Sender)
            .WithMany(x => x.TicketMessages)
            .HasForeignKey(x => x.SenderId);
        
        #endregion
    }
}