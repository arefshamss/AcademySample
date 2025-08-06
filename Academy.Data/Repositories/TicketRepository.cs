using Academy.Data.Context;
using Academy.Data.Repositories.Generics;
using Academy.Domain.Contracts;
using Academy.Domain.Models.Ticket;

namespace Academy.Data.Repositories;

public class TicketRepository(AcademyContext context) : EfRepository<Ticket>(context), ITicketRepository;