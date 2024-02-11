using BlazorApp.Models.ResponseModels;
using Newtonsoft.Json;

namespace BlazorApp.Shared.Models.ResponseModel.Session
{
    public class UserBaseResponseModel : IdResponseModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
