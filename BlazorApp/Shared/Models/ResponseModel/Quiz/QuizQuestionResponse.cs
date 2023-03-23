using BlazorApp.Models.ResponseModels.Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModels.Quiz
{
    public class QuizQuestionResponse
    {
        public int Id { get; set; }

        public string Question { get; set; }

        public List<QuizAnswerResponse> Answers { get; set; } = new List<QuizAnswerResponse>();

        public DateTime CreatedAt { get; set; }

        public int QuizId { get; set; }
    }
}
