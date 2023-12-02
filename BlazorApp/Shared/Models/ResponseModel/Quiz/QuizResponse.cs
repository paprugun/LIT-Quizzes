using BlazorApp.Shared.Models.ResponseModel.Quiz;
using BlazorApp.Shared.Models.ResponseModel.User;
using BlazorApp.Shared.Models.ResponseModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModels.Quiz
{
    public class QuizResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public double TimeToPass { get; set; }
        public TopicResponse Topic { get; set; }
        public List<QuizQuestionResponse> Questions { get; set; } = new List<QuizQuestionResponse>();
        public List<UserResultResponse> UsersJoined { get; set; } = new List<UserResultResponse>();

    }
}
