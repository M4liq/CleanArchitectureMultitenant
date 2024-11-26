using Application.Common.Dtos;
using Application.Common.Interfaces.Core;
using Application.Common.Interfaces.Settings;
using Infrastructure.Settings;
using MailKit.Net.Smtp;
using MimeKit;

namespace Infrastructure.Services.Core;

public class EmailClient : IEmailClient
{
    private readonly ISmtpClientSettings _smtpClientSettings;
    
    public EmailClient(ISmtpClientSettings smtpClientSettings)
    {
        _smtpClientSettings = smtpClientSettings;
    }

    public async Task<EmailClientResultDto> SendEmailAsync(EmailMessageDto emailMessageDto)
    {
        using var client = new SmtpClient();

        try
        {
            await client.ConnectAsync(_smtpClientSettings.Host, _smtpClientSettings.Port, _smtpClientSettings.UseSsl);
            await client.AuthenticateAsync(_smtpClientSettings.Login, _smtpClientSettings.Password);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailMessageDto.From, _smtpClientSettings.Login));
            message.To.Add(new MailboxAddress(emailMessageDto.ToName, emailMessageDto.ToEmail));
            message.Subject = emailMessageDto.Subject;
            message.Body = new TextPart("html")
            {
                Text = emailMessageDto.Content
            };

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch
        {
            return new EmailClientResultDto
            {
                IsSuccessful = false
            };
        }

        return new EmailClientResultDto
        {
            IsSuccessful = true
        };
    }
}