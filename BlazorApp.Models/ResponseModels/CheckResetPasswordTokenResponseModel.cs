using Newtonsoft.Json;

namespace BlazorApp.Models.ResponseModels
{
    public class CheckResetPasswordTokenResponseModel
    {
        [JsonRequired]
        [JsonProperty("isValid")]
        public bool IsValid { get; set; }
    }
}