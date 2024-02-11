using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.RequestModels
{
    public class MailRequestModel
    {
        public string Addressee { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}
