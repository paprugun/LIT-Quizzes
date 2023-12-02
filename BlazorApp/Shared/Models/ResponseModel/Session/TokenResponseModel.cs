using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.Session
{
    public class TokenResponseModel
    {
        [JsonRequired]
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonRequired]
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonRequired]
        [JsonProperty("expireDate")]
        public string ExpireDate { get; set; }

        [JsonRequired]
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
