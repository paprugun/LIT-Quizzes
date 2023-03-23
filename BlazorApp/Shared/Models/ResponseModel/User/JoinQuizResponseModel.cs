using BlazorApp.Shared.Models.ResponseModels.Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.User
{
    public class JoinQuizResponseModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public QuizResponse CurrentQuiz { get; set; }
    }
}
