using APIAssinaturaBarbearia.Application.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAssinaturaBarbearia.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private readonly SmtpConfigs _smtpConfigs;
        public EmailService(IOptions<SmtpConfigs> smtpConfigs)
        {
            _smtpConfigs = smtpConfigs.Value;
        }
        public async Task EnviarEmailAsync(string email, string titulo, string corpo)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Luis", _smtpConfigs.Remetente));
            message.To.Add(new MailboxAddress("Destinatário", email));
            message.Subject = titulo;
            message.Body = new TextPart("html")
            {
                Text = corpo,
            };

            var security = MailKit.Security.SecureSocketOptions.StartTls;

            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync("smtp.gmail.com", 587, security);
                await smtpClient.AuthenticateAsync(_smtpConfigs.Remetente, _smtpConfigs.Senha);
                await smtpClient.SendAsync(message);
                await smtpClient.DisconnectAsync(true);
            }
        }
    }
}
