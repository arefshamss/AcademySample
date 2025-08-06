using Academy.Domain.Contracts.Generics;
using Academy.Domain.Models.EmailSmtp;

namespace Academy.Domain.Contracts;

public interface IEmailSmtpRepository:IEfRepository<EmailSmtp,short>;