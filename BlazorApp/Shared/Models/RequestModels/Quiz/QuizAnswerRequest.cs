using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace BlazorApp.Shared.Models.RequestModels.Quiz
{
    public class QuizAnswerRequest
    {
        [Required(ErrorMessage = "Text is required")]
        [JsonProperty("text")]
        public string Text { get; set; }

        [Required(ErrorMessage = "IsCorrect is required")]
        [JsonProperty("isCorrect")]
        public bool IsCorrect { get; set; }

        [Required(ErrorMessage = "QuestionId is required")]
        [JsonProperty("questionId")]
        public int QuestionId { get; set; }

    }
}
