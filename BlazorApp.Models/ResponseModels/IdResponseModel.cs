using Newtonsoft.Json;

namespace BlazorApp.Models.ResponseModels
{
    public class IdResponseModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
