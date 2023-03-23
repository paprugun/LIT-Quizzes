using Newtonsoft.Json;

namespace BlazorApp.Models.ResponseModels.Session
{
    public class RegisterUsingPhoneResponseModel
    {
        [JsonRequired]
        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonRequired]
        [JsonProperty("sMSSent")]
        public bool SMSSent { get; set; }
    }
}
