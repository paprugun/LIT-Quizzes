using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.ResponseModels.Course
{
    public class UserCourseResultResponseModel
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int UserId { get; set; }

        public string CourseName { get; set; }

        public string CourseTopic { get; set; }

        public string UserName { get; set; }

        public int CountOfDoneSteps { get; set; }

        public int CountOfLeftSteps { get; set; }
    }
}
