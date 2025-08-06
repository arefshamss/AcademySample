using Academy.Domain.Contracts.Generics;
using Academy.Domain.Models.Captcha;

namespace Academy.Domain.Contracts;

public interface ICaptchaRepository:IEfRepository<Captcha,short>;