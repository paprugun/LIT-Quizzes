using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.User
{
    public class SmallUserResultResponse
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string QuizName { get; set; }
        public int CountOfCorrectAnswers { get; set; }
        public int CountOfIncorrectAnswers { get; set; }
        public int ResultMark { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
