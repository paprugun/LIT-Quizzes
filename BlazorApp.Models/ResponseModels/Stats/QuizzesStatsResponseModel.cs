using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.ResponseModels.Stats
{
    public class QuizzesStatsResponseModel
    {
        public int QuizzesCount { get; set; }
        public int QuestionsCount { get; set; }
        public int AnswersCount { get; set; }
    }
}
