using Academy.Data.Context;
using Academy.Data.Repositories.Generics;
using Academy.Domain.Contracts;
using Academy.Domain.Models.EmailSmtp;

namespace Academy.Data.Repositories;

public class EmailSmtpRepository(AcademyContext context):EfRepository<EmailSmtp,short>(context), IEmailSmtpRepository;