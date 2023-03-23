using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.ResponseModels.Quiz
{
	public class QuizQuestionResponseModel
	{
        public int Id { get; set; }

        public string Question { get; set; }

        public List<QuizAnswerResponseModel> Answers { get; set; } = new List<QuizAnswerResponseModel>();

        public DateTime CreatedAt { get; set; }

        public int QuizId { get; set; }
    }
}
