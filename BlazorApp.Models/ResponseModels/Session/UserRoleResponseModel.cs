using Newtonsoft.Json;

namespace BlazorApp.Models.ResponseModels.Session
{
    public class UserRoleResponseModel : UserResponseModel
    {
        [JsonProperty("role")]
        public string Role { get; set; }
    }
}
