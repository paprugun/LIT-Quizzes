using BlazorApp.Shared.Models.RequestModels.Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.RequestModels.User
{
    public class PassQuizRequestModel
    {
        public int QuizId { get; set; }
        public List<QuizAnswerRequest> UserAnswers { get; set; } = new List<QuizAnswerRequest>();
    }
}
