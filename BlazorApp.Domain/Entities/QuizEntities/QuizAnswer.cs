using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Domain.Entities.QuizEntities
{
    public class QuizAnswer : IEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime CreatedAt { get; set; }
        public int QuestionId { get; set; }

    }
}
