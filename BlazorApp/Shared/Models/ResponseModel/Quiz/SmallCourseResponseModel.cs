using BlazorApp.Models.ResponseModels.Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.Quiz
{
    public class SmallCourseResponseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Topic { get; set; }

        public int LessonsCount { get; set; }

        public int Difficult { get; set; }

        public string Language { get; set; }

    }
}
