using Academy.Application.Mapper.TicketMessageMappings;
using Academy.Application.Mapper.UserMappings;
using Academy.Domain.Enums.Ticket;
using Academy.Domain.Models.Ticket;
using Academy.Domain.ViewModels.Ticket.Admin;
using Academy.Domain.ViewModels.Ticket.Client;

namespace Academy.Application.Mapper.TicketMappings;

public static class TicketMapper
{
    #region Admin

    #region MapToAdminTicketViewModel

    public static AdminTicketViewModel MapToAdminTicketViewModel(this Ticket model) =>
        new()
        {
            Id = model.Id,
            TicketStatus = model.TicketStatus,
            TicketPriority = model.TicketPriority,
            TicketSection = model.TicketSection,
            CreatedDate = model.CreatedDate,
            Title = model.Title,
            IsDeleted = model.IsDeleted,
            User = model.User.MapToAdminUserViewModel(),
            ReadBySupporter = model.ReadBySupporter,
            ReadByUser = model.ReadByUser,
            Messages = model.TicketMessages?.Select(x => x.MapToAdminTicketMessageViewModel()).ToList()
        };

    #endregion

    #region MapToTicket - Create

    public static Ticket MapToTicket(this AdminCreateTicketViewModel model) =>
        new()
        {
            Title = model.Title,
            UserId = model.UserId,
            ReadBySupporter = true,
            TicketStatus = TicketStatus.PendingForUserAnswer,
            TicketPriority = model.TicketPriority,
            TicketSection = model.TicketSection,
        };

    #endregion
    
    #region MapToAdminUpdateTicketViewModel

    public static AdminUpdateTicketViewModel MapToAdminUpdateTicketViewModel(this Ticket model) =>
        new()
        {
            Id = model.Id,
            Title = model.Title,
            TicketPriority = model.TicketPriority,
            TicketSection = model.TicketSection,
        };

    #endregion
    
    #region MapToTicket - Update

    public static void MapToTicket(this Ticket model, AdminUpdateTicketViewModel viewModel)
    {
        model.Id = viewModel.Id;
        model.Title = viewModel.Title;
        model.TicketPriority = viewModel.TicketPriority;
        model.TicketSection = viewModel.TicketSection;
    }

    #endregion

    #endregion

    #region Client

    #region MapToClientTicketViewModel

    public static ClientTicketViewModel MapToClientTicketViewModel(this Ticket model) =>
        new()
        {
            Id = model.Id,
            TicketStatus = model.TicketStatus,
            TicketPriority = model.TicketPriority,
            TicketSection = model.TicketSection,
            CreatedDate = model.CreatedDate,
            Title = model.Title,
            IsDeleted = model.IsDeleted,
            User = model.User.MapToClientUserViewModel(),
            ReadBySupporter = model.ReadBySupporter,
            ReadByUser = model.ReadByUser,
            Messages = model.TicketMessages?.Select(x => x.MapToClientTicketMessageViewModel()).ToList()
        };

    #endregion

    #region MapToTicket - Create

    public static Ticket MapToTicket(this ClientCreateTicketViewModel model) =>    
        new()
        {
            Title = model.Title,
            UserId = model.UserId,
            ReadBySupporter = true,
            TicketStatus = TicketStatus.InProgress,
            TicketPriority = model.TicketPriority,
            TicketSection = model.TicketSection
        };

    #endregion

    #endregion
}