using Application.Common.Dtos;

namespace Application.Common.Core;

public interface IEmailClient
{
    Task<EmailClientResultDto> SendEmailAsync(EmailMessageDto emailMessageDto);
}