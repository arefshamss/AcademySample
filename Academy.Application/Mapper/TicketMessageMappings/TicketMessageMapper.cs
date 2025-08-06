using Academy.Application.Statics;
using Academy.Application.Extensions;
using Academy.Application.Mapper.UserMappings;
using Academy.Domain.Models.Ticket;
using Academy.Domain.ViewModels.TicketMessage.Admin;
using Academy.Domain.ViewModels.TicketMessage.Client;

namespace Academy.Application.Mapper.TicketMessageMappings;

public static class TicketMessageMapper
{
    #region Admin

    #region MapToAdminTicketMessageViewModel

    public static AdminTicketMessageViewModel MapToAdminTicketMessageViewModel(this TicketMessage model) =>
        new()
        {
            Id = model.Id,
            Message = model.Message.Trim(),
            TicketId = model.TicketId,
            SenderId = model.SenderId,
            AttachmentUrl = !model.Attachment.IsNullOrEmptyOrWhiteSpace() ? FilePaths.TicketMessageAttachmentFile + model.Attachment : null,
            ReadByUser = model.ReadByUser,
            ReadBySupporter = model.ReadBySupporter,
            CreatedDate = model.CreatedDate,
            Sender = model.Sender.MapToAdminUserViewModel()
        };

    #endregion
    
    #region MapTicketMessage

    public static TicketMessage MapTicketMessage(this AdminCreateTicketMessageViewModel model) =>
        new()
        {
            SenderId = model.SenderId,
            TicketId = model.TicketId,
            Message = model.Message,
            ReadBySupporter = true
        };

    #endregion
    
    #endregion

    #region Client
    
    #region MapToClientTicketMessageViewModel

    public static ClientTicketMessageViewModel MapToClientTicketMessageViewModel(this TicketMessage model) =>
        new()
        {
            Id = model.Id,
            Message = model.Message.Trim(),
            TicketId = model.TicketId,
            SenderId = model.SenderId,
            AttachmentUrl = !model.Attachment.IsNullOrEmptyOrWhiteSpace() ? FilePaths.TicketMessageAttachmentFile + model.Attachment : null,
            ReadByUser = model.ReadByUser,
            ReadBySupporter = model.ReadBySupporter,
            CreatedDate = model.CreatedDate,
            Sender = model.Sender.MapToClientUserViewModel()
        };

    #endregion

    #region MapTicketMessage

    public static TicketMessage MapTicketMessage(this ClientCreateTicketMessageViewModel model) =>  
        new()
        {
            SenderId = model.SenderId,
            TicketId = model.TicketId,
            Message = model.Message,
            ReadByUser = true
        };

    #endregion

    #endregion
}