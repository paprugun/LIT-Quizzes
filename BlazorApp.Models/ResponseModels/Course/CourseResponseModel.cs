using BlazorApp.Models.ResponseModels.Quiz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.ResponseModels.Course
{
    public class CourseResponseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Difficult { get; set; }

        public string[] Topics { get; set; }

        public string Language { get; set; }

        public string ContentURLs { get; set; }

        public List<int> Quizzes { get; set; }
    }
}
