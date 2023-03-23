using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.RequestModels.Quiz
{
	public class QuizAnswerRequestModel
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
