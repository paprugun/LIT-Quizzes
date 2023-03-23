using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.RequestModels.Quiz
{
	public class QuizQuestionRequestModel
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
