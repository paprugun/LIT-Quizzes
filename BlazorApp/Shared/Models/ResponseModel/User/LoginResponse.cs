using BlazorApp.Shared.Models.ResponseModel.Session;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.User
{
    public class LoginResponse
    {
        [JsonProperty("user")]
        public UserRoleResponse User { get; set; }

        [JsonRequired]
        [JsonProperty("token", NullValueHandling = NullValueHandling.Ignore)]
        public TokenResponse Token { get; set; }
    }
}
