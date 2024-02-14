using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.RequestModels.Course
{
    public class CourseRequestModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Topic { get; set; }

        public int Difficult { get; set; }

        public string Language { get; set; }

        public List<LessonRequestModel> Lessons { get; set; }
    }
}
