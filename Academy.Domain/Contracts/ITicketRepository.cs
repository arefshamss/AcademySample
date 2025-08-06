using Academy.Domain.Contracts.Generics;
using Academy.Domain.Models.Ticket;

namespace Academy.Domain.Contracts;

public interface ITicketRepository : IEfRepository<Ticket>;