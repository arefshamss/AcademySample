using Academy.Application.DTOs;
using Academy.Domain.Shared;

namespace Academy.Application.Services.Interfaces;

public interface IEmailSenderService
{


     Task<Result> SendAsync(SendMailRequestDto mailRequest);


}