using ECommerce.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Infrastructure.Services
{
    public sealed class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMessageAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
          await  SendMessageAsync(new[] { to }, subject, body);
        }

        public async Task SendMessageAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;

            foreach (var to in tos)
                mail.To.Add(to);

            mail.Subject = subject;
            mail.Body = body;
            mail.From = new(_configuration["Mail:Username"], "E-Commerce", Encoding.UTF8);

            SmtpClient smtp = new();
            smtp.Credentials = new NetworkCredential(_configuration["Mail:UserName"], _configuration["Mail:Password"]);
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Host = _configuration["Mail:Host"];
            await smtp.SendMailAsync(mail);
        }
    }
}
