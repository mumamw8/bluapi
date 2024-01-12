using System;
namespace Core.Interfaces;

public interface IMailService
{
    Task SendEmailAsync(string toEmail, string subject, string content);
}

