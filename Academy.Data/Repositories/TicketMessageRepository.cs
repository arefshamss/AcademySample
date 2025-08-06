using Academy.Data.Context;
using Academy.Data.Repositories.Generics;
using Academy.Domain.Contracts;
using Academy.Domain.Models.Ticket;

namespace Academy.Data.Repositories;

public class TicketMessageRepository(AcademyContext context) : EfRepository<TicketMessage>(context), ITicketMessageRepository;