using Academy.Data.Context;
using Academy.Data.Repositories.Generics;
using Academy.Domain.Contracts;
using Academy.Domain.Models.Captcha;

namespace Academy.Data.Repositories;

public class CaptchaRepository(AcademyContext context):EfRepository<Captcha,short>(context), ICaptchaRepository;