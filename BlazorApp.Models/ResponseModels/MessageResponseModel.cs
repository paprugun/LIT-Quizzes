using Newtonsoft.Json;

namespace BlazorApp.Models.ResponseModels
{
    public class MessageResponseModel
    {
        public MessageResponseModel(string message)
        {
            Message = message;
        }

        [JsonRequired]
        public string Message { get; set; }
    }
}