using Academy.Data.Context;
using Academy.Data.Repositories.Generics;
using Academy.Domain.Contracts;
using Academy.Domain.Models.Captcha;

namespace Academy.Data.Repositories;

public class CaptchaSettingRepository(AcademyContext context):EfRepository<CaptchaSetting,short>(context), ICaptchaSettingRepository;