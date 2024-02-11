using BlazorApp.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Services.Interfaces
{
    public interface IEmailService
    {
        Task Send(MailRequestModel model);
    }
}
