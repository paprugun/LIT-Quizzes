using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;


namespace BlazorApp.Shared.Models.RequestModels.Quiz
{
    public class QuizRequest
    {

        [Required(ErrorMessage = "name is required")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "author is required")]
        [JsonProperty("author")]
        public string Author { get; set; }

        [Required(ErrorMessage = "timeToPass is required")]
        [JsonProperty("timeToPass")]
        public double TimeToPass { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "topic is required")]
        [JsonProperty]
        public string Topic { get; set; }
    }
}
