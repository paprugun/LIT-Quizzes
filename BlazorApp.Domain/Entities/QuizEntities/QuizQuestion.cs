using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Domain.Entities.QuizEntities
{
    public class QuizQuestion : IEntity
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string Question { get; set; }
        public DateTime CreatedAt { get; set; }

        [InverseProperty("Questions")]
        public virtual Quiz Quiz { get; set; }

        [InverseProperty("Question")]
        public virtual ICollection<QuizAnswer> Answers { get; set; }

    }
}
