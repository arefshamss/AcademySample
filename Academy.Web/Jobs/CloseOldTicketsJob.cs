using Academy.Application.Services.Interfaces;
using Quartz;

namespace Academy.Web.Jobs;

public sealed class CloseOldTicketsJob(ITicketService ticketService) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await ticketService.CloseOldTicketsAsync();
    }
}