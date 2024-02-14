using BlazorApp.Shared.Models.ResponseModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.Course
{
    public class LessonResponseModel
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public double Time { get; set; }

        public string URL { get; set; }

        public List<(int, string)> Quizzes { get; set; }

        public List<SmallUserResultResponse> Results { get; set; }
    }
}
