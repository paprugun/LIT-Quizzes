using Newtonsoft.Json;

namespace BlazorApp.Models.ResponseModels.Session
{
    public class SingleTokenResponseModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
