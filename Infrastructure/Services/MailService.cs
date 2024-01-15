using System;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Services;

public class MailService : IMailService
{
    private readonly IConfiguration _configuration;

    public MailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string content)
    {
        var apiKey = _configuration["SendGridApiKey"];
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("mumamw8@outlook.com", "CashCal");
        var to = new EmailAddress(toEmail);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
        var response = await client.SendEmailAsync(msg);
    }
}

