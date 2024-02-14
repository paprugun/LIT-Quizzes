using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Domain.Entities.QuizEntities
{
    public class CourseLesson : IEntity<int>
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string URL { get; set; }

        public double Time { get; set; }

        [InverseProperty("Lessons")]
        public virtual Course Course { get; set; }

        [InverseProperty("Lesson")]
        public virtual ICollection<LessonQuizzes> Quizzes { get; set; }

    }
}
