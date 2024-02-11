using BlazorApp.Shared.Models.ResponseModels.Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.User
{
    public class UserResultResponseModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public QuizResponse Quiz { get; set; }
        public int CountOfCorrectAnswers { get; set; }
        public int CountOfIncorrectAnswers { get; set; }
        public int ResultMark { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
