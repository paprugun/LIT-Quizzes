using Newtonsoft.Json;

namespace BlazorApp.Shared.Models.ResponseModel
{
    public class IdResponseModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
