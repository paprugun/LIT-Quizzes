using BlazorApp.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Domain.Entities.QuizEntities
{
    public class CoursesQuizzes : IEntity<int>
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int QuizId { get; set; }

        [InverseProperty("Quizzes")]
        public virtual Course Course { get; set; }

        [InverseProperty("Courses")]
        public virtual Quiz Quiz { get; set; }

    }
}
