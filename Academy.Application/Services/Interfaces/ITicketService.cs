using Academy.Domain.Shared;
using Academy.Domain.ViewModels.Ticket.Admin;
using Academy.Domain.ViewModels.Ticket.Client;

namespace Academy.Application.Services.Interfaces;

public interface ITicketService
{
    #region Admin

    Task<Result<AdminFilterTicketsViewModel>> FilterAsync(AdminFilterTicketsViewModel filter);
    
    Task<Result<int>> CreateAsync(AdminCreateTicketViewModel model);

    Task<Result<AdminUpdateTicketViewModel>> FillModelForUpdateAsync(int id);

    Task<Result> UpdateAsync(AdminUpdateTicketViewModel model);

    Task<Result> DeleteOrRecoverAsync(int id);

    Task<Result<AdminTicketViewModel>> GetByIdAsync(int id);

    Task<Result> ToggleCloseTicketAsync(int id, int userId);

    Task CloseOldTicketsAsync();
    
    #endregion
    
    #region Client

    Task<Result<ClientFilterTicketsViewModel>> FilterAsync(ClientFilterTicketsViewModel filter);

    Task<Result<int>> CreateAsync(ClientCreateTicketViewModel model);

    Task<Result<ClientTicketViewModel>> GetByIdForUserPanelAsync(int id);

    Task<int> GetUnreadTicketsCountAsync(int userId);

    #endregion
}