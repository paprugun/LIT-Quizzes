using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.Course
{
    public class SmallLessonResponseModel
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

    }
}
