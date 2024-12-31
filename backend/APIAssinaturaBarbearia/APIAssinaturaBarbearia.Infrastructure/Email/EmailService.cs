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
            message.From.Add(new MailboxAddress(_smtpConfigs.Nome, _smtpConfigs.Remetente));
            message.To.Add(new MailboxAddress("Destinatário", email));
            message.Subject = titulo;
            message.Body = new TextPart("html")
            {
                Text = corpo,
            };

            var security = _smtpConfigs.Security == "STARTTLS" ?  MailKit.Security.SecureSocketOptions.StartTls : MailKit.Security.SecureSocketOptions.SslOnConnect;

            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync(_smtpConfigs.Server, _smtpConfigs.Port, security);
                await smtpClient.AuthenticateAsync(_smtpConfigs.Remetente, _smtpConfigs.Senha);
                await smtpClient.SendAsync(message);
                await smtpClient.DisconnectAsync(true);
            }
        }
    }
}
