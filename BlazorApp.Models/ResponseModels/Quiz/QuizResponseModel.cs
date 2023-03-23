using BlazorApp.Models.ResponseModels.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.ResponseModels.Quiz
{
	public class QuizResponseModel
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public double TimeToPass { get; set; }
        public TopicResponseModel Topic { get; set; }
        public List<QuizQuestionResponseModel> Questions { get; set; } = new List<QuizQuestionResponseModel>();
        public List<UserResultResponseModel> UsersJoined { get; set; } = new List<UserResultResponseModel>();
    }
}
