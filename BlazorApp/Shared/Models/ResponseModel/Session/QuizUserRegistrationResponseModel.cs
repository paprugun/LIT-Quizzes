using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.Session
{
    public class RegistrationResponse
    {
        [JsonRequired]
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
