﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.Course
{
    public class CourseResponseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Difficult { get; set; }

        public string Topic { get; set; }

        public string Language { get; set; }

        public List<LessonResponseModel> Lessons { get; set; }
    }
}
