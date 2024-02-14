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

        public string Topic { get; set; }

        public string Language { get; set; }

        public List<LessonResponseModel> Lessons { get; set; }

    }
}
