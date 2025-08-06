using Academy.Domain.Shared;
using Academy.Domain.ViewModels.TicketMessage.Admin;
using Academy.Domain.ViewModels.TicketMessage.Client;

namespace Academy.Application.Services.Interfaces;

public interface ITicketMessageService
{
    Task<Result> CreateAsync(AdminCreateTicketMessageViewModel model);
    
    Task<Result> CreateAsync(ClientCreateTicketMessageViewModel model);
}