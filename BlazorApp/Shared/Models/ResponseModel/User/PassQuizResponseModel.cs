using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModels.User
{
    public class PassQuizResponseModel
    {
        public int ResultId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public double FinalMark { get; set; }
        public int CorrectAnswersCount { get; set; }
        public int InCorrectAnswersCount { get; set; }

    }
}
