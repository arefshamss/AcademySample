using Academy.Domain.Contracts.Generics;
using Academy.Domain.Models.Ticket;

namespace Academy.Domain.Contracts;

public interface ITicketMessageRepository : IEfRepository<TicketMessage>;