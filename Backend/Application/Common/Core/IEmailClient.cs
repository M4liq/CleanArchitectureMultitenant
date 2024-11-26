using Application.Common.Dtos;

namespace Application.Common.Interfaces.Core;

public interface IEmailClient
{
    Task<EmailClientResultDto> SendEmailAsync(EmailMessageDto emailMessageDto);
}