using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Domain.Entities.QuizEntities
{
    public class Topic : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [InverseProperty("Topic")]
        public virtual ICollection<Quiz> Quizzes { get; set; }

        [InverseProperty("Topic")]
        public virtual ICollection<Course> Courses { get; set; }
    }
}
