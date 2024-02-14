using BlazorApp.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Domain.Entities.QuizEntities
{
    public class Quiz : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TopicId { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public double? TimeToPass { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("TopicId")]
        [InverseProperty("Quizzes")]
        public virtual Topic Topic { get; set; }

        [InverseProperty("Quiz")]
        public virtual ICollection<LessonQuizzes> Lessons { get; set; }

        [InverseProperty("Quiz")]
        public virtual ICollection<QuizQuestion> Questions { get; set; }

        [InverseProperty("Quiz")]
        public virtual ICollection<UsersResults> Users { get; set; }
    }
}
