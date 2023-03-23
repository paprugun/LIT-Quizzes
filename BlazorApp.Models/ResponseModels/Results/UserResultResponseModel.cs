using BlazorApp.Models.ResponseModels.Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.ResponseModels.Results
{
    public class UserResultResponseModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
		public QuizResponseModel Quiz { get; set; }
		public int CountOfCurrentAnswers { get; set; }
        public int CountOfIncorrectAnswers { get; set; }
        public int ResultMark { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
