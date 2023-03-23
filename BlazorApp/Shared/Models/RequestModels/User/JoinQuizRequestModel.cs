using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.RequestModels.User
{
    public class JoinQuizRequestModel
    {
        [Required(ErrorMessage = "Код обов`язковий")]
        [JsonProperty("quizId")]
        public int QuizId { get; set; }

    }
}
