using BlazorApp.Common.Exceptions;
using BlazorApp.Models.RequestModels;
using BlazorApp.Models.ResponseModels;
using BlazorApp.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Send(MailRequestModel model)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("a98503754@gmail.com"));
                email.To.Add(MailboxAddress.Parse(model.Addressee));

                email.Subject = model.Subject;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = model.Body };

                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587);
                smtp.Authenticate("a98503754@gmail.com", "xdjf eyej bxrd crku");
                smtp.Send(email);
                smtp.Disconnect(true);
                smtp.Dispose();
            }
            catch (Exception)
            {
                throw new CustomException(System.Net.HttpStatusCode.InternalServerError, "email", "Error while proccesing an email sending");
            }
            
        }
    }
}
