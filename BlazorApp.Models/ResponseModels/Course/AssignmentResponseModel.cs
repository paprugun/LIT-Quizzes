using BlazorApp.Models.ResponseModels.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.ResponseModels.Course
{
    public class AssignmentResponseModel
    {
        public int Id { get; set; }

        public string CourseName { get; set; }

        public string[] Topics { get; set; }

        public string Language { get; set; }

        public string ContentURLs { get; set; }

        public List<SmallUserResultResponseModel> Results { get; set; }

        public List<(int, string)> NotFinishedQuizzes { get; set; }
    }
}
