using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.Quiz
{
    public class CourseRequestModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ContentURLs { get; set; }

        public int[] QuizzesIds { get; set; }

        public int Difficult { get; set; }

        public string Language { get; set; }
    }
}
