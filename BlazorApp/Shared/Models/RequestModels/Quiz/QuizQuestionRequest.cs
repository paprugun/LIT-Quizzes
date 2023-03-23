using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace BlazorApp.Shared.Models.RequestModels.Quiz
{
    public class QuizQuestionRequest
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Question is required")]
        [JsonProperty("question")]
        public string Question { get; set; }

        [Required(ErrorMessage = "QuizId is required")]
        [JsonProperty("quizId")]
        public int QuizId { get; set; }
    }
}
