using Newtonsoft.Json;

namespace BlazorApp.Models.ResponseModels.Session
{
    public class RegisterResponseModel
    {
        [JsonRequired]
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}