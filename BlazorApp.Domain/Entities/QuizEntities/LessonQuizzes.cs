using BlazorApp.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Domain.Entities.QuizEntities
{
    public class LessonQuizzes : IEntity<int>
    {
        public int Id { get; set; }

        public int LessonId { get; set; }

        public int QuizId { get; set; }

        [InverseProperty("Quizzes")]
        public virtual CourseLesson Lesson { get; set; }

        [InverseProperty("Lessons")]
        public virtual Quiz Quiz { get; set; }

    }
}
