﻿using BlazorApp.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Domain.Entities.QuizEntities
{
    public class Course : IEntity<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TopicId { get; set; }

        public string Description { get; set; }

        [DefaultValue(1)]
        public int Difficult { get; set; }

        [DefaultValue("Англійська")]
        public string Language { get; set; }

        #region Navigation Properties

        [ForeignKey("TopicId")]
        [InverseProperty("Courses")]
        public virtual Topic Topic { get; set; }

        [InverseProperty("Course")]
        public virtual ICollection<CourseLesson> Lessons { get; set; }

        [InverseProperty("Course")]
        public virtual ICollection<UsersCourses> Users { get; set; }
        #endregion
    }
}
