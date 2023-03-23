using BlazorApp.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Domain.Entities.QuizEntities
{
    public class UsersResults : IEntity<int>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int QuizId { get; set; }
        public double ResultMark { get; set; }
        public int CountOfCorrectAnswers { get; set; }
        public int CountOfIncorrectAnswers { get; set; }
        public DateTime JoinedAt { get; set; }
        public DateTime FinishedAt { get; set; }

        [InverseProperty("Users")]
        public virtual Quiz Quiz { get; set; }

        [InverseProperty("QuizzesResults")]
        public virtual ApplicationUser User { get; set; }
    }
}
