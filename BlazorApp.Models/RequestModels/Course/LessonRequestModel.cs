using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.RequestModels.Course
{
    public class LessonRequestModel
    {
        public int CourseId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int[] QuizzesIds { get; set; }

        public string URL { get; set; }

        public double Time { get; set; }

    }
}
